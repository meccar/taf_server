using Microsoft.AspNetCore.Identity;
using taf_server.Domain.Model;
using taf_server.Presentations.Dtos.UserAccount;

namespace taf_server.Domain.Interfaces;

/// <summary>
/// Defines the contract for user account command operations in the repository.
/// </summary>
/// <remarks>
/// This interface provides methods for managing user account data, including 
/// checking for existing accounts, creating new accounts, and assigning roles 
/// to users. Implementations of this interface should handle the necessary 
/// persistence logic.
/// </remarks>
public interface IUserAccountCommandRepository
{
    /// <summary>
    /// Checks if user account data already exists.
    /// </summary>
    /// <param name="userAccountData">The user account data to check (e.g., email or phone number).</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains 
    /// a boolean value indicating whether the user account data exists.</returns>
    Task<bool> IsUserAccountDataExisted(string userAccountData);
    /// <summary>
    /// Creates a new user account asynchronously.
    /// </summary>
    /// <param name="createUserAccountDto">The data transfer object containing user account details.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains 
    /// the newly created <see cref="UserAccountModel"/>.</returns>
    Task<UserAccountModel> CreateUserAsync(CreateUserAccountDto createUserAccountDto);
    /// <summary>
    /// Adds a user to one or more roles asynchronously.
    /// </summary>
    /// <param name="user">The user account aggregate to be added to roles.</param>
    /// <param name="roles">An enumerable collection of role names to assign to the user.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddUserToRolesAsync(Domain.Aggregates.UserAccountAggregate user, IEnumerable<string> roles);
}
