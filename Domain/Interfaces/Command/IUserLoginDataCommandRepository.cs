using Domain.Entities;
using Domain.Model;
using Domain.SeedWork.Results;

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
    Task<UserLoginDataResult> CreateUserLoginDataAsync(UserLoginDataModel userLoginDataDto);
}