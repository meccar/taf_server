using Domain.Entities;
using Shared.Model;

namespace Domain.Interfaces;

public interface ITokenRepository
{
    Task<UserTokenModel> GenerateTokenPair(UserAccountAggregate user);
}