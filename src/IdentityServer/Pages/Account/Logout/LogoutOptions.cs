// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


namespace IdentityServer.Pages.Account.Logout;

/// <summary>
/// Provides configuration options for the logout process, including whether to show the logout prompt 
/// and whether to automatically redirect after sign-out.
/// </summary>
public static class LogoutOptions
{
    /// <summary>
    /// Gets a value indicating whether the logout prompt should be displayed to the user.
    /// If set to <c>true</c>, users will be shown a prompt asking for confirmation before logging out.
    /// </summary>
    public static readonly bool ShowLogoutPrompt = true;

    /// <summary>
    /// Gets a value indicating whether the user should be automatically redirected after sign-out.
    /// If set to <c>true</c>, the user is automatically redirected to a specified URI after logging out.
    /// </summary>
    public static readonly bool AutomaticRedirectAfterSignOut = true;
}
