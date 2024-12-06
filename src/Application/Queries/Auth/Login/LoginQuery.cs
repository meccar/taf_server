using Domain.SeedWork.Query;
using Shared.Dtos.Authentication.Login;
using Shared.Model;

namespace Application.Queries.Auth.Login;

/// <summary>
/// Query used to authenticate a user during the login process.
/// Contains the user's credentials (email and password) to validate the login attempt.
/// </summary>
public class LoginQuery : IQuery<LoginResponseDto>
{
    /// <summary>
    /// Gets or sets the user's email address used for authentication.
    /// </summary>
    /// <value>
    /// The email address provided by the user during login.
    /// </value>
    public string Email { get; set; }
    /// <summary>
    /// Gets or sets the user's password used for authentication.
    /// </summary>
    /// <value>
    /// The password provided by the user during login.
    /// </value>
    public string Password { get; set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="LoginQuery"/> class.
    /// </summary>
    /// <param name="userLoginDataModel">The user login data transfer object containing the user's email and password.</param>
    public LoginQuery(
        LoginUserRequestDto userLoginDataModel
        ) =>
        (Email, Password) = (userLoginDataModel.Email, userLoginDataModel.Password);
    
}