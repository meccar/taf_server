using Domain.Model;

namespace Domain.Interfaces.Query;

public interface IUserLoginDataQueryRepository
{
    /// <summary>
    /// Checks if user login data already exists for the specified login credential.
    /// </summary>
    /// <param name="loginCredential">The login credential to check (e.g., email or username).</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains 
    /// a boolean value indicating whether the user login data exists.</returns>
    Task<bool> IsUserLoginDataExisted(string loginCredential);
    Task<UserLoginDataModel> FindOneByEmail(string email);

    Task<bool> IsUserAccountDataExisted(UserLoginDataModel userLoginDataModel);
}