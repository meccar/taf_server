using Application.Dtos.Authentication.Login;
using Domain.Model;
using Domain.SeedWork.Query;

namespace Application.Queries.Auth.Login;

public class LoginQuery : IQuery<UserAccountModel>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public LoginQuery(LoginUserRequestDto dto) =>
        (Email, Password) = (dto.Email, dto.Password);
    
}