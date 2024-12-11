// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

namespace IdentityServer.Pages.Consent;

/// <summary>
/// Represents the input data from the user when granting or denying consent for access to resources.
/// </summary>
public class InputModel
{
    /// <summary>
    /// Gets or sets the value of the button clicked by the user ("yes" or "no").
    /// </summary>
    public string? Button { get; set; }

    /// <summary>
    /// Gets or sets the collection of scopes that the user has consented to.
    /// </summary>
    public IEnumerable<string> ScopesConsented { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets a value indicating whether the user's consent should be remembered for future requests.
    /// </summary>
    public bool RememberConsent { get; set; } = true;

    /// <summary>
    /// Gets or sets the URL to which the user will be redirected after providing consent.
    /// </summary>
    public string? ReturnUrl { get; set; }

    /// <summary>
    /// Gets or sets an optional description that the user may provide along with their consent.
    /// </summary>
    public string? Description { get; set; }
}