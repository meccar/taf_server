using Shared.Model;
using Shared.Results;

namespace Domain.Interfaces;

public interface IUserAccountRepository
{
    Task<Result<UserAccountModel>> CreateUserAccountAsync(UserAccountModel userAccountModel);
    Task<bool> IsUserLoginDataExisted(UserAccountModel userAccountModel);
    Task<bool> ValidateUserLoginData(string email, string password);
}