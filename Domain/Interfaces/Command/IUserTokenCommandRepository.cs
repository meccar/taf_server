using Domain.Model;

namespace Domain.Interfaces.Command;

public interface IUserTokenCommandRepository
{
    Task<UserTokenModel?> CreateUserTokenAsync(UserTokenModel request, UserLoginDataModel userLoginDataModel);
    Task<bool> UpdateUserTokenAsync(UserTokenModel request);
    Task<List<UserTokenModel>?> GetUserTokensByUserAccountId(string userAccountId);
}