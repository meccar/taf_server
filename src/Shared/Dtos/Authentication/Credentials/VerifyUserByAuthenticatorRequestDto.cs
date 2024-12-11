namespace Shared.Dtos.Authentication.Credentials;

/// <summary>
/// Represents a request to verify a user by their email and authenticator code.
/// </summary>
public class VerifyUserByAuthenticatorRequestDto
{
    /// <summary>
    /// Gets or sets the authenticator Token provided by the user.
    /// </summary>
    public string AuthenticatorToken { get; set; } = null!;
}