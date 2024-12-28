using Domain.Aggregates;
using Microsoft.AspNetCore.Identity;
using Shared.Model;
using Shared.Results;

namespace Domain.Interfaces.User;

/// <summary>
/// Defines the contract for interacting with user account data in the repository.
/// </summary>
/// <remarks>
/// This interface provides methods for creating user accounts, checking if login data exists, 
/// and validating user credentials. It acts as a contract for the repository layer handling 
/// the persistence of user account information.
/// </remarks>
public interface IUserAccountRepository
{
    /// <summary>
    /// Asynchronously creates a new user account in the repository.
    /// </summary>
    /// <param name="userAccountModel">The user account model containing the data to create a new user.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a <see cref="Result{UserAccountModel}"/> object with the created user account.</returns>
    // Task<Result<UserAccountAggregate>> CreateUserAccountAsync(UserAccountAggregate userAccountAggregate, string password);

    Task<IdentityResult> CreateAsync(UserAccountAggregate userAccountAggregate, string password);
    Task<IdentityResult> AddToRoleAsync(UserAccountAggregate userAccountAggregate, string role);
    /// <summary>
    /// Checks if the provided user login data (email and password) already exists in the repository.
    /// </summary>
    /// <param name="userAccountModel">The user account model containing the login data to check.</param>
    /// <returns>A task representing the asynchronous operation. The task result is a boolean indicating whether the login data exists.</returns>
    Task<bool> IsUserLoginDataExisted(UserAccountAggregate userAccountAggregate);

    /// <summary>
    /// Checks if the specified user login data exists in the repository.
    /// </summary>
    /// <param name="userLoginData">The user login data (e.g., email or username) to check for existence.</param>
    /// <returns>A task representing the asynchronous operation. The task result is a boolean indicating whether the login data exists.</returns>
    Task<bool> IsUserLoginDataExisted(string userLoginData);

    /// <summary>
    /// Validates the provided email and password against the stored user account credentials.
    /// </summary>
    /// <param name="email">The email address to validate.</param>
    /// <param name="password">The password to validate.</param>
    /// <returns>A task representing the asynchronous operation. The task result is a boolean indicating whether the credentials are valid.</returns>
    Task<Result> ValidateUserLoginData(string email, string password);

    Task<Result<UserAccountAggregate>> IsExistingAndVerifiedUserAccount(string Eid);
    Task<Result<UserAccountAggregate>> GetCurrentUser(string eid);
    Task<Result<UserAccountAggregate>> GetCurrentUser();
    // Task<Result<UserAccountAggregate>> UpdateUserAccountAsync(UserAccountAggregate userAccountAggregate);
    Task<IdentityResult> UpdateAsync(UserAccountAggregate userAccountAggregate);
}
