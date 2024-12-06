namespace Shared.Dtos.Authentication.Credentials;

/// <summary>
/// Represents the response DTO for a successful login, containing the authentication tokens 
/// and their expiration times, which are returned to the client after a successful authentication process.
/// </summary>
public class VerifyUserResponseDto
{
    /// <summary>
    /// Gets or sets the type of the token (e.g., "Bearer").
    /// This field is required and indicates the authentication scheme that should be used with the access token.
    /// </summary>
    public required string TokenType { get; set; }

    /// <summary>
    /// Gets or sets the access token that is used to authenticate the user on subsequent API requests.
    /// This field is required and represents a bearer token that grants access to protected resources.
    /// </summary>
    public required string AccessToken { get; set; }

    /// <summary>
    /// Gets or sets the expiration time of the access token.
    /// This field is required and represents the time at which the access token will expire, typically in ISO 8601 format.
    /// </summary>
    public required string AccessTokenExpires { get; set; }

    /// <summary>
    /// Gets or sets the refresh token used to obtain a new access token once the current access token expires.
    /// This field is required and is typically used to maintain a session without requiring the user to log in again.
    /// </summary>
    public required string RefreshToken { get; set; }

    /// <summary>
    /// Gets or sets the expiration time of the refresh token.
    /// This field is required and represents the time at which the refresh token will expire, typically in ISO 8601 format.
    /// </summary>
    public required string RefreshTokenExpires { get; set; }
}