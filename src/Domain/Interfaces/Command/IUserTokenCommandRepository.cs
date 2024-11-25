using Domain.Entities;
using Domain.Model;

namespace Domain.Interfaces.Command;

public interface IUserTokenCommandRepository
{
    Task<UserTokenModel?> CreateUserTokenAsync(UserLoginDataEntity user, UserTokenModel request);
    Task<UserTokenModel> UpdateUserTokenAsync(UserLoginDataEntity user, UserTokenModel request);
    // Task<bool> TokenExistsAsync(UserLoginDataEntity user, UserTokenModel token);
    Task<bool> RemoveLoginAndAuthenticationTokenAsync(UserLoginDataEntity userLoginDataModel, UserTokenModel token);
    // Task<bool> TestSignInAsync(UserLoginDataModel user,  UserTokenModel token);
}