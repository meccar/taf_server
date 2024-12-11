// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.Account;

/// <summary>
/// Represents the model for the Access Denied page.
/// This page is shown when the user tries to access a resource they are not authorized to view.
/// </summary>
public class AccessDeniedModel : PageModel
{
    /// <summary>
    /// Handles the GET request for the Access Denied page.
    /// This method is called when the user is redirected to the Access Denied page.
    /// </summary>
    public void OnGet()
    {
        // No specific actions are required for the GET request as the Access Denied page
        // is typically a static page that simply informs the user they do not have the required permissions.
    }
}
