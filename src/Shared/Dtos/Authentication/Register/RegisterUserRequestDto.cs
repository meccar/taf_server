using Shared.Dtos.UserAccount;
using Shared.Dtos.UserProfile;
using Swashbuckle.AspNetCore.Annotations;

namespace Shared.Dtos.Authentication.Register;
public class RegisterUserRequestDto
{
    public RegisterUserRequestDto(CreateUserProfileDto userProfile, CreateUserAccountDto userAccount)
    {
        UserProfile = userProfile;
        UserAccount = userAccount;
    }
    // [Required]
    [SwaggerSchema("User profile details")]
    public CreateUserProfileDto UserProfile { get; set; }

    // [Required]
    [SwaggerSchema("User account details")]
    public CreateUserAccountDto UserAccount { get; set; }
}
