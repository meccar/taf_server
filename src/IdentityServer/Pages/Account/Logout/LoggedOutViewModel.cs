// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

namespace IdentityServer.Pages.Account.Logout;

/// <summary>
/// Represents the view model for the "Logged Out" page, containing information 
/// about the post-logout redirect URI, client name, sign-out iframe URL, 
/// and whether automatic redirection is enabled after sign-out.
/// </summary>
public class LoggedOutViewModel
{
    /// <summary>
    /// Gets or sets the URI to which the user will be redirected after sign-out.
    /// This URI is provided by the client that initiated the logout.
    /// </summary>
    public string? PostLogoutRedirectUri { get; set; }

    /// <summary>
    /// Gets or sets the name of the client that initiated the logout request.
    /// This name is typically displayed on the logout page to inform the user 
    /// of which client application they are logging out from.
    /// </summary>
    public string? ClientName { get; set; }

    /// <summary>
    /// Gets or sets the URL for the iframe used to facilitate federated sign-out 
    /// with external identity providers. If federated sign-out is required, 
    /// this iframe URL is used to complete the logout process with the external provider.
    /// </summary>
    public string? SignOutIframeUrl { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user should be automatically redirected 
    /// after sign-out is completed. If set to <c>true</c>, the user is automatically redirected 
    /// after the logout process finishes.
    /// </summary>
    public bool AutomaticRedirectAfterSignOut { get; set; }
}
