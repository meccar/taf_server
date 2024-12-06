namespace Shared.Dtos.Authentication.Credentials;

/// <summary>
/// Represents a request to verify a user by their email and authenticator code.
/// </summary>
public class VerifyUserByAuthenticatorRequestDto
{
    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    public string Email { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the authenticator Token provided by the user.
    /// </summary>
    public string Token { get; set; } = null!;
}