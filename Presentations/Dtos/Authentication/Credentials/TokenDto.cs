namespace taf_server.Presentations.Dtos.Authentication.Credentials;

public class TokenDto
{
    public string TokenType { get; set; }
    public string AccessToken { get; set; }
    public string AccessTokenExpires { get; set; }
    public string RefreshToken { get; set; }
}