using taf_server.Infrastructure.Entities;

namespace taf_server.Domain.Interfaces.Query;

public interface IUserAccountQueryRepository
{
    /// <summary>
    /// Checks if user account data already exists.
    /// </summary>
    /// <param name="userAccountData">The user account data to check (e.g., email or phone number).</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains 
    /// a boolean value indicating whether the user account data exists.</returns>
    Task<bool> IsUserAccountDataExisted(string userAccountData);
    
    Task<UserAccountEntity> FindOneByEmail(string email);
}