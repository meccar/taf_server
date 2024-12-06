// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.Diagnostics;

/// <summary>
/// The Index page model for handling diagnostics-related functionality.
/// </summary>
[SecurityHeaders]
[Authorize]
public class Index : PageModel
{
    /// <summary>
    /// The view model that will hold user authentication data.
    /// </summary>
    public ViewModel View { get; set; } = default!;

    /// <summary>
    /// Handles the GET request for the Index page.
    /// </summary>
    /// <returns>An IActionResult that represents the response to the GET request.</returns>
    public async Task<IActionResult> OnGet()
    {
        // List of local addresses typically used for loopback or local network
        var localAddresses = new List<string?> { "127.0.0.1", "::1" };

        // Add the server's local IP address if available
        if (HttpContext.Connection.LocalIpAddress != null)
        {
            localAddresses.Add(HttpContext.Connection.LocalIpAddress.ToString());
        }

        // Check if the remote IP is not in the list of local addresses
        if (!localAddresses.Contains(HttpContext.Connection.RemoteIpAddress?.ToString()))
        {
            // Return 404 if the request is from an external address
            return NotFound();
        }

        // Fetch authentication details for the current user
        var authResult = await HttpContext.AuthenticateAsync();
        if (authResult.Principal != null)
        {
            // Set the ViewModel with the user's authentication details
            View = new ViewModel(authResult);
        }

        // Return the page
        return Page();
    }
}
