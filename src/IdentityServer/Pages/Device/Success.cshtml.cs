// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.Device;

/// <summary>
/// Represents the success page model for the device flow.
/// This page is shown after the user has successfully consented to the requested permissions.
/// </summary>
[SecurityHeaders]
[Authorize]
public class SuccessModel : PageModel
{
    /// <summary>
    /// Handles the GET request for the success page.
    /// This action is invoked after the user successfully consents to the device flow authorization.
    /// </summary>
    public void OnGet()
    {
        // Logic to handle any specific actions after successful consent, if required.
    }
}
