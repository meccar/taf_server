using taf_server.Domain.Model;
using taf_server.Domain.Repositories;
using taf_server.Infrastructure.Entities;
using taf_server.Presentations.Dtos.UserLoginData;

namespace taf_server.Domain.Interfaces;

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
    /// Checks if user login data already exists for the specified login credential.
    /// </summary>
    /// <param name="loginCredential">The login credential to check (e.g., email or username).</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains 
    /// a boolean value indicating whether the user login data exists.</returns>
    Task<bool> IsUserLoginDataExisted(string loginCredential);
    /// <summary>
    /// Creates a new user login data entry asynchronously.
    /// </summary>
    /// <param name="userLoginDataDto">The data transfer object containing login data details.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains 
    /// the newly created <see cref="UserLoginDataModel"/>.</returns>
    Task<UserLoginDataModel> CreateUserLoginData(CreateUserLoginDataDto userLoginDataDto);
}