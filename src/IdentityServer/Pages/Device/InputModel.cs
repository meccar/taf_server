// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

namespace IdentityServer.Pages.Device;

/// <summary>
/// Represents the input model for the consent page in the device flow process.
/// This model captures the user's consent selection and other relevant information.
/// </summary>
public class InputModel
{
    /// <summary>
    /// Gets or sets the button that the user clicked ('yes' or 'no').
    /// </summary>
    public string? Button { get; set; }

    /// <summary>
    /// Gets or sets the list of scopes the user has consented to.
    /// These scopes correspond to the permissions requested by the client.
    /// </summary>
    public IEnumerable<string> ScopesConsented { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets a value indicating whether the user wants their consent remembered for future requests.
    /// </summary>
    public bool RememberConsent { get; set; } = true;

    /// <summary>
    /// Gets or sets the return URL to which the user will be redirected after consenting.
    /// </summary>
    public string? ReturnUrl { get; set; }

    /// <summary>
    /// Gets or sets a description provided by the client or user regarding the consent.
    /// This may be used to explain the reason for requesting consent.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the user code for the device flow authentication.
    /// This code is used to identify the device flow request.
    /// </summary>
    public string? UserCode { get; set; }
}
