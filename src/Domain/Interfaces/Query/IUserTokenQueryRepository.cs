using Domain.Entities;
using Shared.Model;

namespace Domain.Interfaces.Query;

public interface IUserTokenQueryRepository
{
    Task<bool> TokenExistsAsync(UserAccountAggregate user, UserTokenModel token);
}