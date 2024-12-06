using Shared.Model;

namespace Domain.Interfaces;

/// <summary>
/// Defines the contract for interacting with JWT-related operations, specifically for generating authentication
/// responses, including JWT tokens and refresh token cookies.
/// </summary>
public interface IJwtRepository
{
    /// <summary>
    /// Generates an authentication response containing a JWT and a refresh token cookie for the specified user.
    /// This method is used to authenticate a user and provide them with an access token as well as a refresh token.
    /// </summary>
    /// <param name="userId">The user identifier (usually the email or username) to generate the authentication tokens for.</param>
    /// <returns>A <see cref="TokenModel"/> containing the generated JWT and refresh token information.</returns>
    Task<TokenModel> GenerateAuthResponseWithRefreshTokenCookie(string userId);
}