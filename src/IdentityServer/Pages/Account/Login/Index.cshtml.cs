// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Application.Queries.Auth.Login;
using Domain.Aggregates;
using Domain.Interfaces;
using Domain.Interfaces.Tokens;
using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MediatR;
using Shared.Dtos.Authentication.Login;

namespace IdentityServer.Pages.Account.Login;

/// <summary>
/// Represents the login page model for the identity server.
/// This model handles the login process, including authentication with external providers and local login.
/// </summary>
[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
    private readonly UserManager<UserAccountAggregate> _userManager;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IEventService _events;
    private readonly IAuthenticationSchemeProvider _schemeProvider;
    private readonly IIdentityProviderStore _identityProviderStore;
    private readonly IMediator _mediator;

    /// <summary>
    /// The view model that is used to display the login page.
    /// </summary>
    public ViewModel View { get; set; } = default!;
    
    /// <summary>
    /// The input model for the login form.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; } = default!;

    /// <summary>
    /// Initializes a new instance of the <see cref="Index"/> page model.
    /// </summary>
    /// <param name="interaction">The interaction service for IdentityServer.</param>
    /// <param name="schemeProvider">The authentication scheme provider.</param>
    /// <param name="identityProviderStore">The identity provider store.</param>
    /// <param name="events">The event service.</param>
    /// <param name="userManager">The user manager for managing user accounts.</param>
    /// <param name="signInManager">The sign-in manager for handling user sign-ins.</param>
    /// <param name="jwtTokenRepository">The JWT token repository for token generation.</param>
    /// <param name="mediator">The mediator for handling application requests.</param>
    public Index(
        IIdentityServerInteractionService interaction,
        IAuthenticationSchemeProvider schemeProvider,
        IIdentityProviderStore identityProviderStore,
        IEventService events,
        UserManager<UserAccountAggregate> userManager,
        SignInManager<UserAccountAggregate> signInManager,
        IJwtRepository jwtTokenRepository,
        IMediator mediator
    )
    {
        _userManager = userManager;
        _interaction = interaction;
        _schemeProvider = schemeProvider;
        _identityProviderStore = identityProviderStore;
        _events = events;
        _mediator = mediator;
    }

    /// <summary>
    /// Handles the GET request to display the login page.
    /// If an external login is required, the user will be redirected to the external login page.
    /// </summary>
    /// <param name="returnUrl">The URL to return to after successful login.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an IActionResult.</returns>
    public async Task<IActionResult> OnGet(string? returnUrl)
    {
        await BuildModelAsync(returnUrl);
        
        if (View.IsExternalLoginOnly)
        {
            // We only have one option for logging in and it's an external provider.
            return RedirectToPage("/ExternalLogin/Challenge", new { scheme = View.ExternalLoginScheme, returnUrl });
        }

        return Page();
    }

    /// <summary>
    /// Handles the POST request to process the login form submission.
    /// It validates the login credentials and handles both successful and failed login attempts.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an IActionResult.</returns>
    public async Task<IActionResult> OnPost()
    {
        // Check if we are in the context of an authorization request
        var context = await _interaction.GetAuthorizationContextAsync(Input.ReturnUrl);

        // The user clicked the "cancel" button
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

        return HandleSuccessfulLogin(context);
    }

    /// <summary>
    /// Attempts to log in the user with the provided credentials.
    /// </summary>
    /// <param name="context">The authorization request context, if any.</param>
    /// <returns>A tuple indicating the success of the login and the user account, if successful.</returns>
    private async Task<(bool Success, UserAccountAggregate? User)> AttemptLogin(AuthorizationRequest? context)
    {
        var loginDto = new LoginUserRequestDto
        {
            Email = Input.Username!,
            Password = Input.Password!,
            RememberUser = Input.RememberLogin
        };
        
        await _mediator.Send(new LoginQuery(loginDto));

        var user = await _userManager.FindByNameAsync(Input.Username!);
        if (user == null)
        {
            await LogFailedLogin(context, "user not found");
            return (false, null);
        }

        await LogSuccessfulLogin(context, user);
        return (true, user);
    }

    /// <summary>
    /// Logs a successful login attempt.
    /// </summary>
    /// <param name="context">The authorization request context, if any.</param>
    /// <param name="user">The user account that was successfully logged in.</param>
    private async Task LogSuccessfulLogin(AuthorizationRequest? context, UserAccountAggregate user)
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

    /// <summary>
    /// Logs a failed login attempt.
    /// </summary>
    /// <param name="context">The authorization request context, if any.</param>
    /// <param name="error">The reason for the failed login attempt.</param>
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

    /// <summary>
    /// Handles the scenario when the user cancels the login process.
    /// </summary>
    /// <param name="context">The authorization request context, if any.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an IActionResult.</returns>
    private async Task<IActionResult> HandleCancelButton(AuthorizationRequest? context)
    {
        if (context is not null)
        {
            // If the user cancels, deny authorization and send an "Access Denied" response
            ArgumentNullException.ThrowIfNull(Input.ReturnUrl, nameof(Input.ReturnUrl));

            await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

            if (context.IsNativeClient())
            {
                // For native clients, the UX is improved by showing a loading page
                return this.LoadingPage(Input.ReturnUrl);
            }

            return Redirect(Input.ReturnUrl ?? "~/");
        }
        return Redirect("~/");
    }

    /// <summary>
    /// Handles the scenario when the login is successful.
    /// Redirects the user to the appropriate URL after login.
    /// </summary>
    /// <param name="context">The authorization request context, if any.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an IActionResult.</returns>
    private IActionResult HandleSuccessfulLogin(AuthorizationRequest? context)
    {
        if (context is not null)
        {
            ArgumentNullException.ThrowIfNull(Input.ReturnUrl, nameof(Input.ReturnUrl));

            if (context.IsNativeClient())
            {
                return this.LoadingPage(Input.ReturnUrl);
            }

            return Redirect(Input.ReturnUrl ?? "~/");
        }

        // If no context, handle as a local page redirect
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
            throw new ArgumentException("invalid return URL");
        }
    }

    /// <summary>
    /// Builds the model for the login page.
    /// </summary>
    /// <param name="returnUrl">The return URL after a successful login.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task BuildModelAsync(string? returnUrl)
    {
        Input = new InputModel { ReturnUrl = returnUrl };

        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
        if (context?.IdP is not null && await _schemeProvider.GetSchemeAsync(context.IdP) is not null)
        {
            HandleIdentityProvider(context);
            return;
        }

        await BuildProvidersModel(context);
    }

    /// <summary>
    /// Handles the identity provider specific setup for the login page.
    /// </summary>
    /// <param name="context">The authorization request context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private void HandleIdentityProvider(AuthorizationRequest context)
    {
        var local = context.IdP == IdentityServerConstants.LocalIdentityProvider;

        View = new ViewModel
        {
            EnableLocalLogin = local,
        };

        Input.Username = context.LoginHint;

        if (!local)
        {
            View.ExternalProviders = new[] { new ViewModel.ExternalProvider(context.IdP!) };
        }
    }

    /// <summary>
    /// Builds the model for external login providers.
    /// </summary>
    /// <param name="context">The authorization request context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task BuildProvidersModel(AuthorizationRequest? context)
    {
        var providers = await GetExternalProviders();
        var allowLocal = true;
        
        if (context?.Client is not null)
        {
            allowLocal = context.Client.EnableLocalLogin;
            if (context.Client.IdentityProviderRestrictions.Count > 0)
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

    /// <summary>
    /// Gets a list of external login providers.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains the list of external providers.</returns>
    private async Task<List<ViewModel.ExternalProvider>> GetExternalProviders()
    {
        var schemes = await _schemeProvider.GetAllSchemesAsync();

        var providers = schemes
            .Where(x => x.DisplayName is not null)
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
