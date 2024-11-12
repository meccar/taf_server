using Microsoft.AspNetCore.Identity;

namespace Domain.Model;

public class TokenModel
{
    public string TokenType  { get; set; }
    public string AccessToken { get; set; } 
    public string AccessTokenExpires { get;  set; }
    public string RefreshToken { get; set; } 
    public string RefreshTokenExpires { get; set; }
    
    public TokenModel(string tokenType, string accessToken, string accessTokenExpires,  string refreshToken, string refreshTokenExpires)
    {
        TokenType = tokenType;
        AccessTokenExpires = accessTokenExpires;
        RefreshTokenExpires = refreshTokenExpires;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}