using Domain.Aggregates;
using Shared.Model;
using Shared.Results;

namespace Domain.Interfaces.Tokens;

/// <summary>
/// Defines the contract for interacting with JWT-related operations, specifically for generating authentication
/// responses, including JWT tokens and refresh token cookies.
/// </summary>
public interface IJwtRepository
{
    /// <summary>
    /// Generates an authentication response containing a JWT and a refresh token cookie for the specified user.
    /// This method authenticates a user and provides them with an access token and a refresh token.
    /// </summary>
    /// <param name="user">The <see cref="UserAccountAggregate"/> representing the user for whom the tokens are generated.</param>
    /// <returns>A <see cref="TokenModel"/> containing the generated JWT and refresh token details.</returns>
    Task<TokenModel> GenerateAuthResponseWithRefreshTokenCookie(UserAccountAggregate user);
    
    /// <summary>
    /// Generates an authentication response containing a JWT and a refresh token cookie for the specified user,
    /// based on the provided user token model.
    /// </summary>
    /// <param name="user">The <see cref="UserAccountAggregate"/> representing the user for whom the tokens are generated.</param>
    /// <param name="token">The <see cref="UserTokenModel"/> containing token-related data for generating the response.</param>
    /// <returns>A <see cref="Result{T}"/> containing a <see cref="TokenModel"/> with the generated JWT and refresh token details.</returns>
    Task<Result<TokenModel>> GenerateAuthResponseWithRefreshTokenCookie(UserAccountAggregate user, UserTokenModel token);
}