// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

namespace IdentityServer.Pages.Ciba;

/// <summary>
/// Represents the view model for presenting consent information in the backchannel authentication process.
/// This includes details about the client requesting access and the available scopes.
/// </summary>
public class ViewModel
{
    /// <summary>
    /// Gets or sets the name of the client requesting consent.
    /// This is typically the name of the application or service requesting access.
    /// </summary>
    public string? ClientName { get; set; }

    /// <summary>
    /// Gets or sets the URL of the client requesting consent.
    /// This URL typically points to the homepage or the privacy policy of the client.
    /// </summary>
    public string? ClientUrl { get; set; }

    /// <summary>
    /// Gets or sets the URL of the logo representing the client.
    /// This image is shown to the user during the consent process to identify the client visually.
    /// </summary>
    public string? ClientLogoUrl { get; set; }

    /// <summary>
    /// Gets or sets a message that will be displayed as part of the consent UI.
    /// This message can be used to provide additional context to the user about the consent request.
    /// </summary>
    public string? BindingMessage { get; set; }

    /// <summary>
    /// Gets or sets the collection of identity scopes that the user can consent to.
    /// These scopes typically relate to the user's identity information (e.g., name, email).
    /// </summary>
    public IEnumerable<ScopeViewModel> IdentityScopes { get; set; } = Enumerable.Empty<ScopeViewModel>();

    /// <summary>
    /// Gets or sets the collection of API scopes that the user can consent to.
    /// These scopes relate to permissions for accessing APIs or other resources on behalf of the user.
    /// </summary>
    public IEnumerable<ScopeViewModel> ApiScopes { get; set; } = Enumerable.Empty<ScopeViewModel>();
}

/// <summary>
/// Represents a single scope available for consent during the backchannel authentication process.
/// A scope can represent a permission to access certain resources or information.
/// </summary>
public class ScopeViewModel
{
    /// <summary>
    /// Gets or sets the name of the scope.
    /// This is typically a string that identifies the scope, such as "openid" or "profile".
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the value of the scope.
    /// This is often the same as the name but may contain additional parameters for certain scopes.
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// Gets or sets the display name of the scope.
    /// This is the human-readable name for the scope, such as "Profile" or "Email".
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the description of the scope.
    /// This is a brief explanation of what the scope allows or represents.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this scope should be emphasized (e.g., highlighted) in the UI.
    /// </summary>
    public bool Emphasize { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this scope is required for the consent process.
    /// Required scopes may not be unchecked by the user.
    /// </summary>
    public bool Required { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user has checked (consented to) this scope.
    /// </summary>
    public bool Checked { get; set; }

    /// <summary>
    /// Gets or sets the collection of resources that the scope applies to.
    /// Resources could include API resources or other protected resources associated with this scope.
    /// </summary>
    public IEnumerable<ResourceViewModel> Resources { get; set; } = Enumerable.Empty<ResourceViewModel>();
}

/// <summary>
/// Represents a resource that is associated with a scope.
/// A resource could be an API or another protected service that the scope grants access to.
/// </summary>
public class ResourceViewModel
{
    /// <summary>
    /// Gets or sets the name of the resource.
    /// This is the identifier for the resource, such as "User API" or "Payment API".
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the display name of the resource.
    /// This is a human-readable name, such as "User Data API" or "Payment Gateway".
    /// </summary>
    public string? DisplayName { get; set; }
}