using Shared.Dtos.UserAccount;
using Shared.Dtos.UserProfile;
using Swashbuckle.AspNetCore.Annotations;

namespace Shared.Dtos.Authentication.Register;

/// <summary>
/// Represents the request data required to register a new user,
/// including both user profile and user account details.
/// </summary>
public class RegisterUserRequestDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterUserRequestDto"/> class.
    /// </summary>
    /// <param name="userProfile">The user profile details.</param>
    /// <param name="userAccount">The user account details.</param>
    public RegisterUserRequestDto(CreateUserProfileDto userProfile, CreateUserAccountDto userAccount)
    {
        UserProfile = userProfile;
        UserAccount = userAccount;
    }
    /// <summary>
    /// Gets or sets the user profile details for the registration process.
    /// </summary>
    // [Required]
    [SwaggerSchema("User profile details")]
    public CreateUserProfileDto UserProfile { get; set; }

    /// <summary>
    /// Gets or sets the user account details for the registration process.
    /// </summary>
    // [Required]
    [SwaggerSchema("User account details")]
    public CreateUserAccountDto UserAccount { get; set; }
}
