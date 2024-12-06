// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.Account.Logout;

/// <summary>
/// Displays the page after a user has logged out, including any necessary details about the logout process
/// such as the client name, post-logout redirect URI, and iframe for federated sign-out.
/// </summary>
[SecurityHeaders]
[AllowAnonymous]
public class LoggedOut : PageModel
{
    private readonly IIdentityServerInteractionService _interactionService;

    /// <summary>
    /// The view model that holds information to be displayed on the logged-out page.
    /// </summary>
    public LoggedOutViewModel View { get; set; } = default!;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggedOut"/> class with the required dependencies.
    /// </summary>
    /// <param name="interactionService">The <see cref="IIdentityServerInteractionService"/> that manages IdentityServer's interaction context.</param>
    public LoggedOut(IIdentityServerInteractionService interactionService)
    {
        _interactionService = interactionService;
    }

    /// <summary>
    /// Handles the HTTP GET request for the logged-out page. It retrieves the context information for the logout process
    /// and prepares the view model with data such as the post-logout redirect URI, client name, and iframe URL for federated sign-out.
    /// </summary>
    /// <param name="logoutId">The logout identifier used to track the context of the logout request.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task OnGet(string? logoutId)
    {
        // Get the context information (e.g., client name, post-logout redirect URI, and iframe URL)
        var logout = await _interactionService.GetLogoutContextAsync(logoutId);

        View = new LoggedOutViewModel
        {
            AutomaticRedirectAfterSignOut = LogoutOptions.AutomaticRedirectAfterSignOut,
            PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
            ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
            SignOutIframeUrl = logout?.SignOutIFrameUrl
        };
    }
}
