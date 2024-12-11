// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.ExternalLogin;

/// <summary>
/// Handles the external authentication challenge by redirecting the user to the specified external authentication provider.
/// This page validates the return URL and initiates the challenge process for the specified external scheme.
/// </summary>
[AllowAnonymous]
[SecurityHeaders]
public class Challenge : PageModel
{
    private readonly IIdentityServerInteractionService _interactionService;

    /// <summary>
    /// Initializes a new instance of the <see cref="Challenge"/> class.
    /// </summary>
    /// <param name="interactionService">The identity server interaction service used to validate return URLs.</param>
    public Challenge(IIdentityServerInteractionService interactionService)
    {
        _interactionService = interactionService;
    }
        
    /// <summary>
    /// Handles the GET request to challenge the user to authenticate via an external provider.
    /// It validates the provided return URL and initiates the authentication process with the specified scheme.
    /// </summary>
    /// <param name="scheme">The authentication scheme to challenge (e.g., Google, Facebook, etc.).</param>
    /// <param name="returnUrl">The URL to redirect the user to after successful authentication.</param>
    /// <returns>An <see cref="IActionResult"/> representing the redirect to the external authentication provider.</returns>
    public IActionResult OnGet(string scheme, string? returnUrl)
    {
        if (string.IsNullOrEmpty(returnUrl)) returnUrl = "~/";

        // validate returnUrl - either it is a valid OIDC URL or back to a local page
        if (Url.IsLocalUrl(returnUrl) == false && _interactionService.IsValidReturnUrl(returnUrl) == false)
        {
            // user might have clicked on a malicious link - should be logged
            throw new ArgumentException("invalid return URL");
        }
            
        // start challenge and roundtrip the return URL and scheme 
        var props = new AuthenticationProperties
        {
            RedirectUri = Url.Page("/externallogin/callback"),
                
            Items =
            {
                { "returnUrl", returnUrl }, 
                { "scheme", scheme },
            }
        };

        return Challenge(props, scheme);
    }
}
