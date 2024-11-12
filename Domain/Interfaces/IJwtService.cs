using System.Security.Claims;
using Domain.Model;

namespace Domain.Interfaces;

public interface IJwtService
{
    Task<TokenModel> ResponseAuthWithAccessTokenAndRefreshTokenCookie(UserLoginDataModel user);
    
}