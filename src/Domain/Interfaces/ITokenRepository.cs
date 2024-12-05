using Domain.Aggregates;
using Domain.Entities;
using Shared.Model;

namespace Domain.Interfaces;

/// <summary>
/// 
/// </summary>
public interface ITokenRepository
{
    Task<UserTokenModel> GenerateTokenPair(UserAccountAggregate user);
}