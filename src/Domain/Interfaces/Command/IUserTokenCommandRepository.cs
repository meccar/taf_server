using Domain.Entities;
using Shared.Model;

namespace Domain.Interfaces.Command;

public interface IUserTokenCommandRepository
{
    Task<UserTokenModel?> CreateUserTokenAsync(UserAccountAggregate user, UserTokenModel request);
    Task<UserTokenModel> UpdateUserTokenAsync(UserAccountAggregate user, UserTokenModel request);
    // Task<bool> TokenExistsAsync(UserAccountAggregate user, UserTokenModel token);
    Task<bool> RemoveLoginAndAuthenticationTokenAsync(UserAccountAggregate userLoginDataModel, UserTokenModel token);
    // Task<bool> TestSignInAsync(UserLoginDataModel user,  UserTokenModel token);
}