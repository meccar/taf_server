using System.Security.Claims;
using Domain.Model;

namespace Domain.Interfaces;

public interface IJwtService
{
    Task<string> ResponseAuthWithAccessTokenAndRefreshTokenCookie(UserAccountModel userAccountModel , bool hasVerify2FA);
}