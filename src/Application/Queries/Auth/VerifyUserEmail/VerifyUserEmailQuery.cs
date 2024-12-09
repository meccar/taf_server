using Domain.SeedWork.Query;
using Shared.Dtos.Authentication.Credentials;

namespace Application.Queries.Auth.VerifyUserEmail;

/// <summary>
/// Query used to verify a user's email based on a provided token.
/// This query is typically used for actions like email verification or password reset validation.
/// </summary>
public class VerifyUserEmailQuery : IQuery<VerifyUserResponseDto>
{
    /// <summary>
    /// Gets or sets the token used for user verification.
    /// This token is typically sent to the user's email for verification purposes.
    /// </summary>
    /// <value>
    /// The verification token (usually received by the user in an email) that is used to validate the user's action.
    /// </value>
    public string Token { get; set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="VerifyUserEmailQuery"/> class.
    /// </summary>
    /// <param name="token">The token used for verifying the user's email, typically received from an external source like email.</param>
    public VerifyUserEmailQuery(
        VerifyUserEmailRequestDto token
        ) =>
        Token = token.UrlToken;
}