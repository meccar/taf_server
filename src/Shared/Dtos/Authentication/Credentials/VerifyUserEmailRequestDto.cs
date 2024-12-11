namespace Shared.Dtos.Authentication.Credentials;

/// <summary>
/// Represents a request to verify a user's email using a verification token.
/// </summary>
public class VerifyUserEmailRequestDto
{
    /// <summary>
    /// Gets or sets the token used to verify the user's email.
    /// This token is typically sent to the user via email to confirm ownership.
    /// </summary>
    public string UrlToken { get; set; } = null!;
}