using taf_server.Presentations.Dtos.Authentication.Credentials;
using taf_server.Presentations.Dtos.UserAccount;

namespace taf_server.Presentations.Dtos.Authentication.Login;

public class LoginResponseDto
{
    public TokenDto Token { get; set; }
    public UserAccountResponseDto UserAccount { get; set; }
}