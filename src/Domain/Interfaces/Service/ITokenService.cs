using Domain.Entities;
using Shared.Model;

namespace Domain.Interfaces;

public interface ITokenService
{
    Task<UserTokenModel> GenerateTokenPair(UserAccountAggregate user);
}