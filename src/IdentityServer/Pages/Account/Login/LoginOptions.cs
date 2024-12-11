// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

namespace IdentityServer.Pages.Account.Login;

/// <summary>
/// Provides configuration options for login functionality in the IdentityServer login page.
/// This includes options for local login, remember login, and error messages.
/// </summary>
public static class LoginOptions
{
    /// <summary>
    /// Indicates whether local login is allowed. If set to <c>true</c>, users can log in with a local username and password.
    /// </summary>
    public static readonly bool AllowLocalLogin = true;

    /// <summary>
    /// Indicates whether the "Remember Me" option is allowed during login. If set to <c>true</c>, users can opt to stay logged in across sessions.
    /// </summary>
    public static readonly bool AllowRememberLogin = true;

    /// <summary>
    /// Specifies the duration for the "Remember Me" feature. If enabled, this determines how long the user stays logged in.
    /// </summary>
    public static readonly TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);

    /// <summary>
    /// The error message displayed when the user provides invalid credentials (username or password).
    /// </summary>
    public static readonly string InvalidCredentialsErrorMessage = "Invalid username or password";
}