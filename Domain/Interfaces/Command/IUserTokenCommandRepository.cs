using Domain.Model;

namespace Domain.Interfaces.Command;

public interface IUserTokenCommandRepository
{
    Task<UserLoginDataModel?> CreateUserTokenAsync(UserTokenModel token);
}