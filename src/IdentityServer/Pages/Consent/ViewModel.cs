// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

namespace IdentityServer.Pages.Consent;

/// <summary>
/// Represents the view model for displaying consent information, including client details and associated scopes.
/// </summary>
public class ViewModel
{
    /// <summary>
    /// Gets or sets the name of the client requesting consent.
    /// </summary>
    public string? ClientName { get; set; }

    /// <summary>
    /// Gets or sets the URL of the client requesting consent.
    /// </summary>
    public string? ClientUrl { get; set; }

    /// <summary>
    /// Gets or sets the URL of the client's logo.
    /// </summary>
    public string? ClientLogoUrl { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the client allows the user to remember their consent.
    /// </summary>
    public bool AllowRememberConsent { get; set; }

    /// <summary>
    /// Gets or sets the list of identity scopes that the user can consent to.
    /// </summary>
    public IEnumerable<ScopeViewModel> IdentityScopes { get; set; } = Enumerable.Empty<ScopeViewModel>();

    /// <summary>
    /// Gets or sets the list of API scopes that the user can consent to.
    /// </summary>
    public IEnumerable<ScopeViewModel> ApiScopes { get; set; } = Enumerable.Empty<ScopeViewModel>();
}

/// <summary>
/// Represents a scope that the user can consent to, including information like its name, description, and associated resources.
/// </summary>
public class ScopeViewModel
{
    /// <summary>
    /// Gets or sets the name of the scope.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the value associated with the scope.
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// Gets or sets the display name of the scope.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the description of the scope.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the scope should be emphasized in the UI.
    /// </summary>
    public bool Emphasize { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the scope is required.
    /// </summary>
    public bool Required { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the scope has been checked/consented to by the user.
    /// </summary>
    public bool Checked { get; set; }

    /// <summary>
    /// Gets or sets the list of resources associated with this scope.
    /// </summary>
    public IEnumerable<ResourceViewModel> Resources { get; set; } = Enumerable.Empty<ResourceViewModel>();
}

/// <summary>
/// Represents a resource associated with a scope, including the name and display name of the resource.
/// </summary>
public class ResourceViewModel
{
    /// <summary>
    /// Gets or sets the name of the resource.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the display name of the resource.
    /// </summary>
    public string? DisplayName { get; set; }
}