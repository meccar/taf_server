using Microsoft.AspNetCore.Identity;

namespace Domain.Model;

public class UserTokenModel
{
    public string TokenType { get; set; }
    public string AccessToken { get; set; }
    public string AccessTokenExpires { get; set; }
    public string RefreshToken { get; set; }
    public string RefreshTokenExpires { get; set; }
}
