using Domain.SeedWork.Query;
using Shared.Dtos.Authentication.Credentials;
using Shared.Model;

namespace Application.Queries.Auth.VerifyUser;

/// <summary>
/// Query used to verify a user's email based on a provided token.
/// This query is typically used for actions like email verification or password reset validation.
/// </summary>
public class VerifyUserQuery : IQuery<VerifyUserEmailRequestDto>
{
    /// <summary>
    /// Gets or sets the token used for user verification.
    /// This token is typically sent to the user's email for verification purposes.
    /// </summary>
    /// <value>
    /// The verification token (usually received by the user in an email) that is used to validate the user's action.
    /// </value>
    public VerifyUserEmailRequestDto Token { get; set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="VerifyUserQuery"/> class.
    /// </summary>
    /// <param name="token">The token used for verifying the user's email, typically received from an external source like email.</param>
    public VerifyUserQuery(
        VerifyUserEmailRequestDto token
        ) =>
        Token = token;
}