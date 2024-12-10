using Domain.SeedWork.Query;
using Shared.Dtos.Authentication.Credentials;

namespace Application.Queries.Auth.VerifyUser;

/// <summary>
/// Represents a query to verify a user by their authenticator token.
/// </summary>
public class VerifyUserByAuthenticatorQuery : IQuery<VerifyUserResponseDto>
{
    /// <summary>
    /// Gets or sets the authenticator token for verifying the user.
    /// </summary>
    public string AuthenticatorToken { get; set; }
    
    /// <summary>
    /// Gets or sets the URL token for verifying the user.
    /// </summary>
    public string UrlToken { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="VerifyUserByAuthenticatorQuery"/> class.
    /// </summary>
    /// <param name="verifyUserEmailRequestDto">The email request data for the user verification.</param>
    /// <param name="verifyUserByAuthenticatorRequestDto">The authenticator token request data for user verification.</param>
    public VerifyUserByAuthenticatorQuery(
        VerifyUserEmailRequestDto verifyUserEmailRequestDto,
        VerifyUserByAuthenticatorRequestDto verifyUserByAuthenticatorRequestDto
    ) =>
        (
            AuthenticatorToken,
            UrlToken
        ) = (
            verifyUserByAuthenticatorRequestDto.AuthenticatorToken, 
            verifyUserEmailRequestDto.UrlToken
        );
}