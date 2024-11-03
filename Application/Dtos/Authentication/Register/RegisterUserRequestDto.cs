using System.ComponentModel.DataAnnotations;
using Application.Dtos.UserAccount;
using Application.Dtos.UserLoginData;
using Swashbuckle.AspNetCore.Annotations;

namespace Application.Dtos.Authentication.Register;
public class RegisterUserRequestDto(CreateUserAccountDto userAccount, CreateUserLoginDataDto userLogin)
{
    [Required]
    [SwaggerSchema("User account details")]
    public CreateUserAccountDto UserAccount { get; set; } = userAccount;

    [Required]
    [SwaggerSchema("User login details")]
    public CreateUserLoginDataDto UserLogin { get; set; } = userLogin;
}
