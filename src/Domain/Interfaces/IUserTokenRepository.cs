using Domain.Entities;
using Shared.Model;

namespace Domain.Interfaces;

public interface IUserTokenRepository
{
    Task<UserTokenModel?> CreateUserTokenAsync(UserAccountAggregate user, UserTokenModel request);
    Task<UserTokenModel> UpdateUserTokenAsync(UserAccountAggregate user, UserTokenModel request);
    Task<bool> RemoveLoginAndAuthenticationTokenAsync(UserAccountAggregate userLoginDataModel, UserTokenModel token);
    Task<bool> TokenExistsAsync(UserAccountAggregate user, UserTokenModel token);

}