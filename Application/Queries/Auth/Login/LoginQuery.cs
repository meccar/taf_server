using taf_server.Domain.Model;
using taf_server.Domain.SeedWork.Query;
using taf_server.Presentations.Dtos.Authentication.Login;

namespace taf_server.Application.Queries.Auth.Login;

public class LoginQuery : IQuery<UserAccountModel>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public LoginQuery(LoginUserRequestDto dto) =>
        (Email, Password) = (dto.Email, dto.Password);
    
}