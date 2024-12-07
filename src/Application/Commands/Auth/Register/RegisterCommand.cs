﻿using Domain.SeedWork.Command;
using Shared.Dtos.Authentication.Register;
using Shared.Dtos.UserAccount;
using Shared.Dtos.UserProfile;
using Shared.Model;

namespace Application.Commands.Auth.Register;

/// <summary>
/// Represents a command to sign up a new user.
/// </summary>
/// <remarks>
/// This command is used to create a new user account with the provided details.
/// </remarks>
public class RegisterCommand : ICommand<RegisterUserResponseDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterCommand"/> class.
    /// </summary>
    /// <param name="dto">
    /// The data transfer object containing the information necessary for user registration.
    /// </param>
    // public RegisterCommand(UserAccountModel userAccountModel, UserLoginDataModel userLoginDataModel) => 
    //     (UserAccountModel, UserLoginDataModel) = (userAccountModel, userLoginDataModel);

    public RegisterCommand(CreateUserProfileDto userProfileModel, CreateUserAccountDto userAccountModel) => 
        (UserProfileModel, UserAccountModel) = (userProfileModel, userAccountModel);
    /// <summary>
    /// Gets or sets the user login information for the new account.
    /// </summary>
    /// <value>
    /// An instance of <see cref="CreateUserLoginDataDto"/> representing the user's login details.
    /// </value>
    public CreateUserProfileDto UserProfileModel { get; set; }

    /// <summary>
    /// Gets or sets the user account information for the new account.
    /// </summary>
    /// <value>
    /// An instance of <see cref="CreateUserAccountDto"/> representing the user's account details.
    /// </value>
    public CreateUserAccountDto UserAccountModel { get; set; }
}
