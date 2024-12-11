// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.Redirect;

/// <summary>
/// Handles the redirection logic for the <see cref="Index"/> page.
/// This page validates the provided redirect URI and either returns the page or redirects to an error page if the URI is invalid.
/// </summary>
[AllowAnonymous]
public class IndexModel : PageModel
{
    /// <summary>
    /// Gets or sets the redirect URI to which the user will be sent after a successful action.
    /// </summary>
    public string? RedirectUri { get; set; }

    /// <summary>
    /// Handles the GET request to the page, validating the provided redirect URI.
    /// </summary>
    /// <param name="redirectUri">The redirect URI passed from the client application.</param>
    /// <returns>An IActionResult representing the page or a redirection to an error page if the URI is invalid.</returns>
    public IActionResult OnGet(string? redirectUri)
    {
        // Validate if the provided redirect URI is a valid local URL.
        if (!Url.IsLocalUrl(redirectUri))
        {
            // If not a valid local URL, redirect to the error page.
            return RedirectToPage("/Home/Error/Index");
        }

        // If valid, set the RedirectUri property and display the page.
        RedirectUri = redirectUri;
        return Page();
    }
}
