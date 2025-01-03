// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using System.Security.Claims;
using Domain.Aggregates;
using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.ExternalLogin;

/// <summary>
/// Handles the external login callback from external identity providers. This page processes the authentication result,
/// creates or updates the user, and signs the user in locally.
/// </summary>
[AllowAnonymous]
[SecurityHeaders]
public class Callback : PageModel
{
    private readonly UserManager<UserAccountAggregate> _userManager;
    private readonly SignInManager<UserAccountAggregate> _signInManager;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly ILogger<Callback> _logger;
    private readonly IEventService _events;

    /// <summary>
    /// Initializes a new instance of the <see cref="Callback"/> class.
    /// </summary>
    /// <param name="interaction">The identity server interaction service.</param>
    /// <param name="events">The event service to raise login events.</param>
    /// <param name="logger">The logger to log events and errors.</param>
    /// <param name="userManager">The user manager to handle user-related operations.</param>
    /// <param name="signInManager">The sign-in manager to handle user sign-in operations.</param>
    public Callback(
        IIdentityServerInteractionService interaction,
        IEventService events,
        ILogger<Callback> logger,
        UserManager<UserAccountAggregate> userManager,
        SignInManager<UserAccountAggregate> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _interaction = interaction;
        _logger = logger;
        _events = events;
    }
        
    /// <summary>
    /// Handles the GET request to process the external authentication result, create or update the user, and sign in the user.
    /// </summary>
    /// <returns>An action result that may redirect or render a page.</returns>
    public async Task<IActionResult> OnGet()
    {
        // read external identity from the temporary cookie
        var result = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
        
        if (result.Succeeded != true)
        {
            _logger.LogError($"External authentication failed: { result.Failure?.Message }");
            _logger.LogError($"Exception details: { result.Failure?.InnerException }");
            throw new InvalidOperationException($"External authentication error: { result.Failure }");
        }

        var externalUser = result.Principal ?? 
            throw new InvalidOperationException("External authentication produced a null Principal");
		
        if (_logger.IsEnabled(LogLevel.Debug))
        {
            var externalClaims = externalUser.Claims.Select(c => $"{c.Type}: {c.Value}");
            // var externalClaims = externalUser.Identity;
            _logger.ExternalClaims(externalClaims);
        }

        // lookup our user and external provider info
        // try to determine the unique id of the external user (issued by the provider)
        // the most common claim type for that are the sub claim and the NameIdentifier
        // depending on the external provider, some other claim type might be used
        var userIdClaim = externalUser.FindFirst(JwtClaimTypes.Subject) ??
                          externalUser.FindFirst(JwtClaimTypes.Id) ??
                          externalUser.FindFirst(ClaimTypes.NameIdentifier) ??
                          throw new InvalidOperationException("Unknown userid");

        var provider = result.Properties.Items["scheme"] ?? throw new InvalidOperationException("Null scheme in authentiation properties");
        var providerUserId = userIdClaim.Value;

        // find external user
        var user = await _userManager.FindByLoginAsync(provider, providerUserId);
        if (user == null)
        {
            // this might be where you might initiate a custom workflow for user registration
            // in this sample we don't show how that would be done, as our sample implementation
            // simply auto-provisions new external user
            user = await AutoProvisionUserAsync(provider, providerUserId, externalUser.Claims);
        }

        // this allows us to collect any additional claims or properties
        // for the specific protocols used and store them in the local auth cookie.
        // this is typically used to store data needed for signout from those protocols.
        var additionalLocalClaims = new List<Claim>();
        var localSignInProps = new AuthenticationProperties();
        CaptureExternalLoginContext(result, additionalLocalClaims, localSignInProps);
            
        // issue authentication cookie for user
        await _signInManager.SignInWithClaimsAsync(user, localSignInProps, additionalLocalClaims);

        // delete temporary cookie used during external authentication
        await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

        // retrieve return URL
        var returnUrl = result.Properties.Items["returnUrl"] ?? "~/";

        // check if external login is in the context of an OIDC request
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
        await _events.RaiseAsync(new UserLoginSuccessEvent(provider, providerUserId, user.EId, user.UserName, true, context?.Client.ClientId));
        Telemetry.Metrics.UserLogin(context?.Client.ClientId, provider);

        if (context != null)
        {
            if (context.IsNativeClient())
            {
                // The client is native, so this change in how to
                // return the response is for better UX for the end user.
                return this.LoadingPage(returnUrl);
            }
        }

        return Redirect(returnUrl);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1851:Possible multiple enumerations of 'IEnumerable' collection", Justification = "<Pending>")]
    private async Task<UserAccountAggregate> AutoProvisionUserAsync(string provider, string providerUserId, IEnumerable<Claim> claims)
    {
        var sub = Ulid.NewUlid().ToString();

        var enumerable = claims as Claim[] ?? claims.ToArray();
        
        var email = enumerable.FirstOrDefault(x => x.Type == JwtClaimTypes.Email)?.Value ??
                    enumerable.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            
        var userEntity = await _userManager.FindByEmailAsync(email!);
        var userProfileId = userEntity?.Id ?? 0;
        
        var user = new UserAccountAggregate
        {
            EId = sub,
            UserName = email, // don't need a username, since the user will be using an external provider to login
            UserProfileId = userProfileId
        };

        // email
        user.Email = email;

        // create a list of claims that we want to transfer into our store
        var filtered = new List<Claim>();

        // user's display name
        var name = enumerable.FirstOrDefault(x => x.Type == JwtClaimTypes.Name)?.Value ??
                   enumerable.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        if (name != null)
        {
            filtered.Add(new Claim(JwtClaimTypes.Name, name));
        }
        else
        {
            var first = enumerable.FirstOrDefault(x => x.Type == JwtClaimTypes.GivenName)?.Value ??
                        enumerable.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
            var last = enumerable.FirstOrDefault(x => x.Type == JwtClaimTypes.FamilyName)?.Value ??
                       enumerable.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;
            if (first != null && last != null)
            {
                filtered.Add(new Claim(JwtClaimTypes.Name, first + " " + last));
            }
            else if (first != null)
            {
                filtered.Add(new Claim(JwtClaimTypes.Name, first));
            }
            else if (last != null)
            {
                filtered.Add(new Claim(JwtClaimTypes.Name, last));
            }
        }

        var identityResult = await _userManager.CreateAsync(user);
        if (!identityResult.Succeeded) throw new InvalidOperationException(identityResult.Errors.First().Description);

        if (filtered.Count != 0)
        {
            identityResult = await _userManager.AddClaimsAsync(user, filtered);
            if (!identityResult.Succeeded) throw new InvalidOperationException(identityResult.Errors.First().Description);
        }

        identityResult = await _userManager.AddLoginAsync(user, new UserLoginInfo(provider, providerUserId, provider));
        if (!identityResult.Succeeded) throw new InvalidOperationException(identityResult.Errors.First().Description);

        return user;
    }

    // if the external login is OIDC-based, there are certain things we need to preserve to make logout work
    // this will be different for WS-Fed, SAML2p or other protocols
    private static void CaptureExternalLoginContext(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
    {
        ArgumentNullException.ThrowIfNull(externalResult.Principal, nameof(externalResult.Principal));

        // capture the idp used to login, so the session knows where the user came from
        localClaims.Add(new Claim(JwtClaimTypes.IdentityProvider, externalResult.Properties?.Items["scheme"] ?? "unknown identity provider"));

        // if the external system sent a session id claim, copy it over
        // so we can use it for single sign-out
        var sid = externalResult.Principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
        if (sid != null)
        {
            localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
        }

        // if the external provider issued an id_token, we'll keep it for signout
        var idToken = externalResult.Properties?.GetTokenValue("id_token");
        if (idToken != null)
        {
            localSignInProps.StoreTokens(new[] { new AuthenticationToken { Name = "id_token", Value = idToken } });
        }
    }
}
