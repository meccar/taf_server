using Domain.Model;

namespace Domain.Interfaces.Command;

public interface IUserTokenCommandRepository
{
    Task<UserTokenModel?> CreateUserTokenAsync(UserTokenModel request);
    Task<bool> UpdateUserTokenAsync(UserTokenModel request);
    Task<List<UserTokenModel>?> TokenExistsAsync(string userId, TokenModel token);
}