// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

namespace IdentityServer.Pages.Device;

    /// <summary>
    /// Represents the view model for the consent page.
    /// This includes information about the client application, the scopes requested, and the consent options.
    /// </summary>
    public class ViewModel
    {
        /// <summary>
        /// Gets or sets the name of the client application.
        /// </summary>
        public string? ClientName { get; set; }

        /// <summary>
        /// Gets or sets the URL of the client application.
        /// </summary>
        public string? ClientUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL of the client's logo.
        /// </summary>
        public string? ClientLogoUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is allowed to remember the consent.
        /// </summary>
        public bool AllowRememberConsent { get; set; }

        /// <summary>
        /// Gets or sets the collection of identity resource scopes.
        /// </summary>
        public IEnumerable<ScopeViewModel> IdentityScopes { get; set; } = Enumerable.Empty<ScopeViewModel>();

        /// <summary>
        /// Gets or sets the collection of API scopes requested by the client.
        /// </summary>
        public IEnumerable<ScopeViewModel> ApiScopes { get; set; } = Enumerable.Empty<ScopeViewModel>();
    }

    /// <summary>
    /// Represents a scope in the consent view.
    /// This includes details about the scope, such as its value, display name, and whether it is required or emphasized.
    /// </summary>
    public class ScopeViewModel
    {
        /// <summary>
        /// Gets or sets the value of the scope (e.g., "openid", "profile", etc.).
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Gets or sets the display name of the scope, which may be used for UI representation.
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the description of the scope, which provides more information to the user.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the scope should be emphasized in the UI.
        /// </summary>
        public bool Emphasize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the scope is required for the operation.
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the scope is selected (checked) by the user.
        /// </summary>
        public bool Checked { get; set; }
    }