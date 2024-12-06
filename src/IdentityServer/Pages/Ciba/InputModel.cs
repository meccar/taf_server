// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

namespace IdentityServer.Pages.Ciba;

/// <summary>
/// Represents the input data for handling a backchannel authentication request,
/// including consented scopes, button click action, and additional user information.
/// </summary>
public class InputModel
{
    /// <summary>
    /// Gets or sets the button clicked by the user, indicating their action (e.g., "yes" or "no").
    /// </summary>
    public string? Button { get; set; }

    /// <summary>
    /// Gets or sets the list of scopes the user has consented to.
    /// This represents the set of permissions or resources the user is granting access to.
    /// </summary>
    public IEnumerable<string> ScopesConsented { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the internal ID of the login request.
    /// This ID uniquely identifies the backchannel authentication request.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the description provided by the user for the consent process.
    /// This is an optional field that may include details about the consent being granted.
    /// </summary>
    public string? Description { get; set; }
}