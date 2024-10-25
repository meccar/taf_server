using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using taf_server.Presentations.Dtos.UserAccount;
using taf_server.Presentations.Dtos.UserLoginData;

namespace taf_server.Presentations.Dtos.Authentication.Register;
public class RegisterUserRequestDto(CreateUserAccountDto userAccount, CreateUserLoginDataDto userLogin)
{
    [Required]
    [SwaggerSchema("User account details")]
    public CreateUserAccountDto UserAccount { get; set; } = userAccount;

    [Required]
    [SwaggerSchema("User login details")]
    public CreateUserLoginDataDto UserLogin { get; set; } = userLogin;
}
