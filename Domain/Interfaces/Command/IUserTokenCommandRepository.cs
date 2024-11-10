using Domain.Model;

namespace Domain.Interfaces.Command;

public interface IUserTokenCommandRepository
{
    Task<UserTokenModel?> CreateUserTokenAsync(UserTokenModel request);
}