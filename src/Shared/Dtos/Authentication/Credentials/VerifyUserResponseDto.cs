namespace Shared.Dtos.Authentication.Credentials;

public class VerifyUserResponseDto
{
    public string TokenType { get; set; }
    public string AccessToken { get; set; }
    public string AccessTokenExpires { get; set; }
    public string RefreshToken { get; set; }
    public string RefreshTokenExpires { get; set; }
}