using Domain.Entities;
using Domain.Model;

namespace Domain.Interfaces;

public interface ITokenService
{
    Task<UserTokenModel> GenerateTokenPair(UserLoginDataEntity user);
}