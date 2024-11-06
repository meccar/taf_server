using Domain.Aggregates;
using Domain.Model;

namespace Domain.Interfaces.Command;

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
    /// Creates a new user account asynchronously.
    /// </summary>
    /// <param name="createUserAccountDto">The data transfer object containing user account details.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains 
    /// the newly created <see cref="UserAccountModel"/>.</returns>
    Task<UserAccountModel> CreateUserAccountAsync(UserAccountModel createUserAccountDto);

}
