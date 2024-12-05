using Domain.SeedWork.Query;
using Shared.Dtos.Authentication.Login;
using Shared.Model;

namespace Application.Queries.Auth.Login;

public class LoginQuery : IQuery<LoginResponseDto>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public LoginQuery(LoginUserRequestDto userLoginDataModel) =>
        (Email, Password) = (userLoginDataModel.Email, userLoginDataModel.Password);
    
}