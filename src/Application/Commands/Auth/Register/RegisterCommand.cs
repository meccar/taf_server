// using Domain.SeedWork.Command;
// using Shared.Dtos.Authentication.Register;
// using Shared.Dtos.UserAccount;
// using Shared.Dtos.UserProfile;
//
// namespace Application.Commands.Auth.Register;
//
// /// <summary>
// /// Command used for registering a new user in the system.
// /// Contains the necessary data for creating both a user profile and a user account.
// /// </summary>
// public class RegisterCommand : ICommand<RegisterUserResponseDto>
// {
//     /// <summary>
//     /// Initializes a new instance of the <see cref="RegisterCommand"/> class.
//     /// </summary>
//     /// <param name="userProfileDto">The user profile data transfer object containing details of the user's profile.</param>
//     /// <param name="userAccountDto">The user account data transfer object containing the user's account information.</param>
//     public RegisterCommand(
//         CreateUserProfileDto userProfileDto,
//         CreateUserAccountDto userAccountDto
//         ) => 
//         (UserProfileModel, UserAccountModel) = (userProfileDto, userAccountDto);
//     /// <summary>
//     /// Gets or sets the user profile model, which holds the user's profile information.
//     /// </summary>
//     /// <value>
//     /// The user profile data transfer object containing details about the user.
//     /// </value>
//     public CreateUserProfileDto UserProfileModel { get; set; }
//     /// <summary>
//     /// Gets or sets the user account model, which holds the user's account information.
//     /// </summary>
//     /// <value>
//     /// The user account data transfer object containing information about the user's account (e.g., username, password).
//     /// </value>
//     public CreateUserAccountDto UserAccountModel { get; set; }
//
// }

using Domain.SeedWork.Command;
using Shared.Dtos.Authentication.Register;
using Shared.Dtos.UserAccount;
using Shared.Dtos.UserProfile;

namespace Application.Commands.Auth.Register;

/// <summary>
/// Command used for registering a new user in the system.
/// Contains the necessary data for creating both a user profile and a user account.
/// </summary>
public class RegisterCommand : ICommand<RegisterUserResponseDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterCommand"/> class.
    /// </summary>
    /// <param name="userProfileDto">The user profile data transfer object containing details of the user's profile.</param>
    /// <param name="userAccountDto">The user account data transfer object containing the user's account information.</param>
    public RegisterCommand(
        CreateUserProfileDto userProfileDto,
        CreateUserAccountDto userAccountDto
    ) => 
    (
        Email,
        Password,
        PhoneNumber,
        FirstName,
        LastName,
        Gender,
        DateOfBirth
    ) = (
        userAccountDto.Email,
        userAccountDto.Password,
        userAccountDto.PhoneNumber,
        userProfileDto.FirstName,
        userProfileDto.LastName,
        userProfileDto.Gender,
        userProfileDto.DateOfBirth
    );

    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Gender { get; set; } = "";
    public string DateOfBirth { get; set; } = "";
}
