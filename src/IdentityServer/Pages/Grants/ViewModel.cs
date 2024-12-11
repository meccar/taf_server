// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

namespace IdentityServer.Pages.Grants;

/// <summary>
/// Represents the view model that holds a collection of grants for the user.
/// </summary>
public class ViewModel
{
    /// <summary>
    /// Gets or sets the collection of grants associated with the user.
    /// </summary>
    public IEnumerable<GrantViewModel> Grants { get; set; } = Enumerable.Empty<GrantViewModel>();
}

/// <summary>
/// Represents a single grant associated with a client, including the details of the grant, its expiration, and related scopes.
/// </summary>
public class GrantViewModel
{
    /// <summary>
    /// Gets or sets the client ID associated with the grant.
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// Gets or sets the name of the client associated with the grant.
    /// </summary>
    public string? ClientName { get; set; }

    /// <summary>
    /// Gets or sets the URL of the client associated with the grant.
    /// </summary>
    public string? ClientUrl { get; set; }

    /// <summary>
    /// Gets or sets the URL of the client's logo associated with the grant.
    /// </summary>
    public string? ClientLogoUrl { get; set; }

    /// <summary>
    /// Gets or sets a description of the grant.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the creation date of the grant.
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// Gets or sets the expiration date of the grant, or null if it does not expire.
    /// </summary>
    public DateTime? Expires { get; set; }

    /// <summary>
    /// Gets or sets the collection of identity grant names associated with the grant.
    /// </summary>
    public IEnumerable<string> IdentityGrantNames { get; set; } = Enumerable.Empty<string>();

    /// <summary>
    /// Gets or sets the collection of API grant names associated with the grant.
    /// </summary>
    public IEnumerable<string> ApiGrantNames { get; set; } = Enumerable.Empty<string>();
}
