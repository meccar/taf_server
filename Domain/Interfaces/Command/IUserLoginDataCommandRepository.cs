using Domain.Entities;
using Domain.Model;

namespace Domain.Interfaces.Command;

/// <summary>
/// Defines the contract for user login data command operations in the repository.
/// </summary>
/// <remarks>
/// This interface provides methods for managing user login data, including 
/// checking for existing login credentials and creating new login data entries. 
/// Implementations of this interface should handle the necessary persistence logic.
/// </remarks>
public interface IUserLoginDataCommandRepository
{
    /// <summary>
    /// Creates a new user login data entry asynchronously.
    /// </summary>
    /// <param name="userLoginDataDto">The data transfer object containing login data details.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains 
    /// the newly created <see cref="UserLoginDataModel"/>.</returns>
    Task<UserLoginDataModel?> CreateUserLoginDataAsync(UserLoginDataModel userLoginDataDto);
    
    /// <summary>
    /// Adds a user to one or more roles asynchronously.
    /// </summary>
    /// <param name="user">The user account aggregate to be added to roles.</param>
    /// <param name="roles">An enumerable collection of role names to assign to the user.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddUserToRolesAsync(UserLoginDataEntity user, IEnumerable<string> roles);
}