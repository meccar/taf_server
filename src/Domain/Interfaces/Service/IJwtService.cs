using Shared.Model;

namespace Domain.Interfaces.Service;

public interface IJwtRepository
{
    Task<TokenModel> GenerateAuthResponseWithRefreshTokenCookie(string userId);

}