using Application.Dtos.Authentication.Credentials;
using Application.Dtos.UserAccount;

namespace Application.Dtos.Authentication.Login;

public class LoginResponseDto
{
    public TokenDto Token { get; set; }
    public UserAccountResponseDto UserAccount { get; set; }
}