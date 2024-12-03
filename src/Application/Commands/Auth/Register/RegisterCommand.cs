using Domain.SeedWork.Command;
using Shared.Dtos.UserAccount;
using Shared.Model;

namespace Application.Commands.Auth.Register;

/// <summary>
/// Represents a command to sign up a new user.
/// </summary>
/// <remarks>
/// This command is used to create a new user account with the provided details.
/// </remarks>
public class RegisterCommand : ICommand<UserProfileModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterCommand"/> class.
    /// </summary>
    /// <param name="dto">
    /// The data transfer object containing the information necessary for user registration.
    /// </param>
    // public RegisterCommand(UserAccountModel userAccountModel, UserLoginDataModel userLoginDataModel) => 
    //     (UserAccountModel, UserLoginDataModel) = (userAccountModel, userLoginDataModel);

    public RegisterCommand(UserProfileModel userProfileModel, UserAccountModel userAccountModel) => 
        (UserProfileModel, UserAccountModel) = (userProfileModel, userAccountModel);
    /// <summary>
    /// Gets or sets the user login information for the new account.
    /// </summary>
    /// <value>
    /// An instance of <see cref="CreateUserLoginDataDto"/> representing the user's login details.
    /// </value>
    public UserProfileModel UserProfileModel { get; set; }

    /// <summary>
    /// Gets or sets the user account information for the new account.
    /// </summary>
    /// <value>
    /// An instance of <see cref="CreateUserAccountDto"/> representing the user's account details.
    /// </value>
    public UserAccountModel UserAccountModel { get; set; }
}
