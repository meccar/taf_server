using Domain.SeedWork.Results;
using Shared.Model;

namespace Domain.Interfaces;

public interface IUserAccountRepository
{
    Task<UserLoginDataResult> CreateUserLoginDataAsync(UserAccountModel userAccountModel);
    Task<bool> IsUserLoginDataExisted(UserAccountModel userAccountModel);
    Task<bool> ValidateUserLoginData(string email, string password);
}