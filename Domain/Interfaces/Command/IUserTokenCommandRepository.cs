using Domain.Model;

namespace Domain.Interfaces.Command;

public interface IUserTokenCommandRepository
{
    Task<UserTokenModel?> CreateUserTokenAsync(UserTokenModel request);
    Task<bool> UpdateUserTokenAsync(UserTokenModel request);
    Task<List<UserTokenModel>?> GetUserTokensByUserAccountId(string userAccountId);
}