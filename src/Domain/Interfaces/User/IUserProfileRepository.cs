using Shared.Model;
using Shared.Results;

namespace Domain.Interfaces.User;

/// <summary>
/// Defines the contract for interacting with user profile data in the repository.
/// </summary>
/// <remarks>
/// This interface provides methods for creating user profiles and retrieving user account status. 
/// It acts as a contract for the repository layer handling the persistence of user profile information.
/// </remarks>
public interface IUserProfileRepository
{
    /// <summary>
    /// Asynchronously creates a new user profile in the repository.
    /// </summary>
    /// <param name="createUserAccountDto">The user profile model containing the data to create a new user profile.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a <see cref="Result{UserProfileModel}"/> object with the created user profile.</returns>
    Task<Result<UserProfileModel>> CreateUserProfileAsync(UserProfileModel createUserAccountDto);

    /// <summary>
    /// Retrieves the status of a user account by its user ID.
    /// </summary>
    /// <param name="userId">The user ID of the account to retrieve the status for.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the status of the user account as a string.</returns>
    Task<string> GetUserAccountStatusAsync(string userId);
}