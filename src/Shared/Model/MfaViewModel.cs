using System.ComponentModel.DataAnnotations;

namespace Shared.Model;

/// <summary>
/// Represents a model for multi-factor authentication (MFA) setup details.
/// </summary>
public class MfaViewModel
{
    /// <summary>
    /// Gets or sets the shared key used for generating MFA codes.
    /// </summary>
    [Required] 
    public string SharedKey { get; set; } = null!;

    /// <summary>
    /// Gets or sets the URI for configuring an authenticator application.
    /// </summary>
    [Required] 
    public string AuthenticatorUri { get; set; } = null!;
}