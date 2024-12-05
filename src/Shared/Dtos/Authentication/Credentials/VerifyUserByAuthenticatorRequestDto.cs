namespace Shared.Dtos.Authentication.Credentials;

public class VerifyUserByAuthenticatorRequestDto
{
    public string Email { get; set; }
    public string Token { get; set; }

}