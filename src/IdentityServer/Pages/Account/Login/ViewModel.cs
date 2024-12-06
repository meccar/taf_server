// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

namespace IdentityServer.Pages.Account.Login;

/// <summary>
/// Represents the view model for the login page, encapsulating login options and external authentication providers.
/// </summary>
public class ViewModel
{
    /// <summary>
    /// Indicates whether the "Remember Me" option is allowed during login.
    /// If <c>true</c>, the user can choose to stay logged in across sessions.
    /// </summary>
    public bool AllowRememberLogin { get; set; } = true;

    /// <summary>
    /// Indicates whether local login (i.e., username/password) is enabled.
    /// If <c>true</c>, users can log in with their username and password.
    /// </summary>
    public bool EnableLocalLogin { get; set; } = true;

    /// <summary>
    /// The list of external authentication providers available for login.
    /// </summary>
    public IEnumerable<ViewModel.ExternalProvider> ExternalProviders { get; set; } = Enumerable.Empty<ExternalProvider>();

    /// <summary>
    /// Filters and returns only the external providers that have a non-empty display name.
    /// </summary>
    public IEnumerable<ViewModel.ExternalProvider> VisibleExternalProviders =>
        ExternalProviders.Where(x => !String.IsNullOrWhiteSpace(x.DisplayName));

    /// <summary>
    /// Determines if only external login is available (i.e., local login is disabled, and there is exactly one external provider).
    /// </summary>
    public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;

    /// <summary>
    /// Gets the authentication scheme for the only external provider if only external login is available.
    /// </summary>
    public string? ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;

    /// <summary>
    /// Represents an external authentication provider with its display name and authentication scheme.
    /// </summary>
    public class ExternalProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalProvider"/> class with the specified authentication scheme and optional display name.
        /// </summary>
        /// <param name="authenticationScheme">The authentication scheme associated with the external provider.</param>
        /// <param name="displayName">The display name for the external provider, or <c>null</c> if not provided.</param>
        public ExternalProvider(string authenticationScheme, string? displayName = null)
        {
            AuthenticationScheme = authenticationScheme;
            DisplayName = displayName;
        }

        /// <summary>
        /// Gets or sets the display name of the external provider.
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the authentication scheme for the external provider.
        /// </summary>
        public string AuthenticationScheme { get; set; }
    }
}