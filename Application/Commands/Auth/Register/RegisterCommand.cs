using taf_server.Presentations.Dtos.Authentication;
using taf_server.Domain.SeedWork.Command;
using taf_server.Domain.SeedWork.Enums.UserAccount;
using taf_server.Presentations.Dtos.UserAccount;
using taf_server.Presentations.Dtos.UserLoginData;

namespace taf_server.Application.Commands.Auth.Register;

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
    public RegisterCommand(RegisterUserRequestDto dto)
        => (UserLogin, UserAccount)
            = (dto.UserLogin, dto.UserAccount);

    /// <summary>
    /// Gets or sets the user login information for the new account.
    /// </summary>
    /// <value>
    /// An instance of <see cref="CreateUserLoginDataDto"/> representing the user's login details.
    /// </value>
    public CreateUserLoginDataDto UserLogin { get; set; }

    /// <summary>
    /// Gets or sets the user account information for the new account.
    /// </summary>
    /// <value>
    /// An instance of <see cref="CreateUserAccountDto"/> representing the user's account details.
    /// </value>
    public CreateUserAccountDto UserAccount { get; set; }
}
