// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Application.Dtos.Authentication.Login;
using Application.Usecases.Auth;
using Domain.Entities;
using Domain.Interfaces.Service;
using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Infrastructure.UseCaseProxy;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IdentityServer.Pages;
using IdentityServer.Pages.Login;
using Telemetry = IdentityServer.Pages.Telemetry;

namespace IdentityServer.Pages.Account.Login;

[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
    private readonly UserManager<UserLoginDataEntity> _userManager;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IEventService _events;
    private readonly IAuthenticationSchemeProvider _schemeProvider;
    private readonly IIdentityProviderStore _identityProviderStore;
    private readonly UseCaseProxy<LoginUsecase, LoginUserRequestDto, LoginResponseDto> _loginUsecase;

    public ViewModel View { get; set; } = default!;
        
    [BindProperty]
    public InputModel Input { get; set; } = default!;
        
    public Index(
        IIdentityServerInteractionService interaction,
        IAuthenticationSchemeProvider schemeProvider,
        IIdentityProviderStore identityProviderStore,
        IEventService events,
        UserManager<UserLoginDataEntity> userManager,
        SignInManager<UserLoginDataEntity> signInManager,
        UseCaseProxy<LoginUsecase, LoginUserRequestDto, LoginResponseDto> loginUsecase,
        IJwtService jwtTokenService
        )
    {
        _userManager = userManager;
        _interaction = interaction;
        _schemeProvider = schemeProvider;
        _identityProviderStore = identityProviderStore;
        _events = events;
        _loginUsecase = loginUsecase;
    }

    public async Task<IActionResult> OnGet(string? returnUrl)
    {
        await BuildModelAsync(returnUrl);
            
        if (View.IsExternalLoginOnly)
        {
            // we only have one option for logging in and it's an external provider
            return RedirectToPage("/ExternalLogin/Challenge", new { scheme = View.ExternalLoginScheme, returnUrl });
        }

        return Page();
    }
        
    public async Task<IActionResult> OnPost()
    {
        // check if we are in the context of an authorization request
        var context = await _interaction.GetAuthorizationContextAsync(Input.ReturnUrl);

        // the user clicked the "cancel" button
        if (Input.Button != "login")
        {
            return await HandleCancelButton(context);
        }

        if (!ModelState.IsValid)
        {
            await BuildModelAsync(Input.ReturnUrl);
            return Page();
        }

        var loginResult = await AttemptLogin(context);
        if (!loginResult.Success)
        {
            ModelState.AddModelError(string.Empty, LoginOptions.InvalidCredentialsErrorMessage);
            await BuildModelAsync(Input.ReturnUrl);
            return Page();
        }

        return await HandleSuccessfulLogin(context);
    }
    
    private async Task<(bool Success, UserLoginDataEntity? User)> AttemptLogin(AuthorizationRequest? context)
    {
        var loginDto = new LoginUserRequestDto
        {
            Email = Input.Username!,
            Password = Input.Password!,
            RememberUser = Input.RememberLogin
        };
        
        var response = await _loginUsecase.GetInstance().Execute(loginDto);
        
        if (response == null)
        {
            await LogFailedLogin(context, "invalid credentials");
            return (false, null);
        }

        var user = await _userManager.FindByNameAsync(Input.Username!);
        if (user == null)
        {
            await LogFailedLogin(context, "user not found");
            return (false, null);
        }

        await LogSuccessfulLogin(context, user);
        return (true, user);
    }

    private async Task LogSuccessfulLogin(AuthorizationRequest? context, UserLoginDataEntity user)
    {
        await _events.RaiseAsync(new UserLoginSuccessEvent(
            user.Email,
            user.EId,
            user.Email,
            clientId: context?.Client.ClientId));
                
        Telemetry.Metrics.UserLogin(
            context?.Client.ClientId,
            IdentityServerConstants.LocalIdentityProvider);
    }

    private async Task LogFailedLogin(AuthorizationRequest? context, string error)
    {
        await _events.RaiseAsync(new UserLoginFailureEvent(
            Input.Username,
            error,
            clientId: context?.Client.ClientId));
            
        Telemetry.Metrics.UserLoginFailure(
            context?.Client.ClientId,
            IdentityServerConstants.LocalIdentityProvider,
            error);
    }
    
    private async Task<IActionResult> HandleCancelButton(AuthorizationRequest? context)
    {
        if (context != null)
        {
            // This "can't happen", because if the ReturnUrl was null, then the context would be null
            ArgumentNullException.ThrowIfNull(Input.ReturnUrl, nameof(Input.ReturnUrl));

            // if the user cancels, send a result back into IdentityServer as if they 
            // denied the consent (even if this client does not require consent).
            // this will send back an access denied OIDC error response to the client.
            await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

            // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
            if (context.IsNativeClient())
            {
                // The client is native, so this change in how to
                // return the response is for better UX for the end user.
                return this.LoadingPage(Input.ReturnUrl);
            }

            return Redirect(Input.ReturnUrl ?? "~/");
        }
        return Redirect("~/");
    }

    private async Task<IActionResult> HandleSuccessfulLogin(AuthorizationRequest? context)
    {
        if (context != null)
        {
            // This "can't happen", because if the ReturnUrl was null, then the context would be null
            ArgumentNullException.ThrowIfNull(Input.ReturnUrl, nameof(Input.ReturnUrl));

            if (context.IsNativeClient())
            {
                // The client is native, so this change in how to
                // return the response is for better UX for the end user.
                return this.LoadingPage(Input.ReturnUrl);
            }

            // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
            return Redirect(Input.ReturnUrl ?? "~/");
        }

        // request for a local page
        if (Url.IsLocalUrl(Input.ReturnUrl))
        {
            return Redirect(Input.ReturnUrl);
        }
        else if (string.IsNullOrEmpty(Input.ReturnUrl))
        {
            return Redirect("~/");
        }
        else
        {
            // user might have clicked on a malicious link - should be logged
            throw new ArgumentException("invalid return URL");
        }
    }
    private async Task BuildModelAsync(string? returnUrl)
    {
        Input = new InputModel { ReturnUrl = returnUrl };
            
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
        if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
        {
            await HandleIdentityProvider(context);
            return;
        }

        await BuildProvidersModel(context);
    }
    
    private async Task HandleIdentityProvider(AuthorizationRequest context)
    {
        var local = context.IdP == IdentityServerConstants.LocalIdentityProvider;

        View = new ViewModel
        {
            EnableLocalLogin = local,
        };

        Input.Username = context.LoginHint;

        if (!local)
        {
            View.ExternalProviders = new[] { new ViewModel.ExternalProvider(context.IdP) };
        }
    }

    private async Task BuildProvidersModel(AuthorizationRequest? context)
    {
        var providers = await GetExternalProviders();
        var allowLocal = true;
        
        if (context?.Client != null)
        {
            allowLocal = context.Client.EnableLocalLogin;
            if (context.Client.IdentityProviderRestrictions?.Count > 0)
            {
                providers = providers.Where(provider => 
                    context.Client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
            }
        }

        View = new ViewModel
        {
            AllowRememberLogin = LoginOptions.AllowRememberLogin,
            EnableLocalLogin = allowLocal && LoginOptions.AllowLocalLogin,
            ExternalProviders = providers.ToArray()
        };
    }

    private async Task<List<ViewModel.ExternalProvider>> GetExternalProviders()
    {
        var schemes = await _schemeProvider.GetAllSchemesAsync();

        var providers = schemes
            .Where(x => x.DisplayName != null)
            .Select(x => new ViewModel.ExternalProvider(
                x.Name,
                x.DisplayName ?? x.Name
            ))
            .ToList();

        var dynamicSchemes = (await _identityProviderStore.GetAllSchemeNamesAsync())
            .Where(x => x.Enabled)
            .Select(x => new ViewModel.ExternalProvider(
                x.Scheme,
                x.DisplayName ?? x.Scheme
            ));
            
        providers.AddRange(dynamicSchemes);
        return providers;
    }
}
