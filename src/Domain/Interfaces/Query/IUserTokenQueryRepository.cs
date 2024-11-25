using Domain.Entities;
using Domain.Model;

namespace Domain.Interfaces.Query;

public interface IUserTokenQueryRepository
{
    Task<bool> TokenExistsAsync(UserLoginDataEntity user, UserTokenModel token);
}