using Domain.Aggregates;
using Microsoft.AspNetCore.Identity;
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
    Task<UserAccountAggregate?> IsExistingAndVerifiedUserAccount(string Eid);
    Task<UserAccountAggregate?> GetCurrentUser(string eid);
    Task<UserAccountAggregate?> GetCurrentUser();
    Task<IdentityResult> UpdateAsync(UserAccountAggregate userAccountAggregate);
}
