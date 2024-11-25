using Domain.Model;
using Domain.SeedWork.Query;

namespace Application.Queries.Auth.Login;

public class LoginQuery : IQuery<TokenModel>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public LoginQuery(UserLoginDataModel userLoginDataModel) =>
        (Email, Password) = (userLoginDataModel.Email, userLoginDataModel.Password);
    
}