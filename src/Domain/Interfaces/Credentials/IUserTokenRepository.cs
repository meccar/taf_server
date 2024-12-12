using Domain.Aggregates;
using Shared.Model;

namespace Domain.Interfaces.Credentials;

    /// <summary>
    /// Defines the contract for interacting with user token data in the repository.
    /// </summary>
    /// <remarks>
    /// This interface provides methods for creating, updating, removing, and checking the existence of user authentication tokens.
    /// It serves as the repository for managing user authentication and authorization tokens within the application.
    /// </remarks>
    public interface IUserTokenRepository
    {
        /// <summary>
        /// Asynchronously creates a new user token for a given user.
        /// </summary>
        /// <param name="user">The user account for which the token is being created.</param>
        /// <param name="request">The token model containing the token details to be created.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the created <see cref="UserTokenModel"/> if successful, or <c>null</c> if creation fails.</returns>
        Task<UserTokenModel?> CreateUserTokenAsync(UserAccountAggregate user, UserTokenModel request);

        /// <summary>
        /// Asynchronously removes a user's login and authentication token.
        /// </summary>
        /// <param name="userLoginDataModel">The user account for which the token is being removed.</param>
        /// <param name="token">The token model that represents the login and authentication token to be removed.</param>
        /// <returns>A task representing the asynchronous operation. The task result is <c>true</c> if the token was successfully removed; otherwise, <c>false</c>.</returns>
        Task<bool> RemoveLoginAndAuthenticationTokenAsync(UserAccountAggregate userLoginDataModel, UserTokenModel token);
    }