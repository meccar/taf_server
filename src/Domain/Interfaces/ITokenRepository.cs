using Domain.Aggregates;
using Shared.Model;

namespace Domain.Interfaces;

/// <summary>
/// Defines the contract for managing tokens related to user authentication.
/// This interface provides a method for generating a pair of tokens (access and refresh tokens) for a user.
/// </summary>
public interface ITokenRepository
{
    /// <summary>
    /// Generates a pair of tokens (access and refresh tokens) for the specified user.
    /// These tokens are used for user authentication and session management.
    /// The access token is typically short-lived and used to authenticate API requests, while the refresh token is used to obtain a new access token when the current one expires.
    /// </summary>
    /// <param name="user">The user for whom the token pair is being generated. The user's data is used to create the tokens.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="UserTokenModel"/> which includes the generated access and refresh tokens.</returns>
    UserTokenModel GenerateTokenPair(UserAccountAggregate user);
}