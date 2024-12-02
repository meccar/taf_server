using Shared.Model;

namespace Domain.Interfaces.Service;

public interface IJwtService
{
    Task<TokenModel> GenerateAuthResponseWithRefreshTokenCookie(string userId);

}