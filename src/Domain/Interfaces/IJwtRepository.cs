using Shared.Model;

namespace Domain.Interfaces;

public interface IJwtRepository
{
    Task<TokenModel> GenerateAuthResponseWithRefreshTokenCookie(string userId);

}