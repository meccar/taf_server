namespace Shared.Dtos.Authentication.Login;

public class LoginResponseDto
{
    // public TokenDto Token { get; set; }
    // public UserAccountResponseDto UserAccount { get; set; }
    public string TokenType { get; set; }
    public string AccessToken { get; set; }
    public string AccessTokenExpires { get; set; }
    public string RefreshToken { get; set; }
    public string RefreshTokenExpires { get; set; }
}