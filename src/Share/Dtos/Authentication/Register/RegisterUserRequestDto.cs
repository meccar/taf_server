using Application.Dtos.UserLoginData;
using Share.Dtos.UserAccount;
using Swashbuckle.AspNetCore.Annotations;

namespace Share.Dtos.Authentication.Register;
public class RegisterUserRequestDto
{
    public RegisterUserRequestDto(CreateUserAccountDto userAccount, CreateUserLoginDataDto userLoginData)
    {
        UserAccount = userAccount;
        UserLoginData = userLoginData;
    }
    // [Required]
    [SwaggerSchema("User account details")]
    public CreateUserAccountDto UserAccount { get; set; }

    // [Required]
    [SwaggerSchema("User login details")]
    public CreateUserLoginDataDto UserLoginData { get; set; }
}
