using Domain.SeedWork.Results;
using Shared.Model;

namespace Domain.Interfaces;

public interface IUserAccountRepository
{
    Task<UserLoginDataResult> CreateUserLoginDataAsync(UserLoginDataModel userLoginDataDto);
    Task<bool> IsUserLoginDataExisted(UserLoginDataModel userLoginDataModel);
    Task<bool> ValidateUserLoginData(string email, string password);
}