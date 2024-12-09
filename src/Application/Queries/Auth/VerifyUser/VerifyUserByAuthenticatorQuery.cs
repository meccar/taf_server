using Domain.SeedWork.Query;
using Shared.Dtos.Authentication.Credentials;

namespace Application.Queries.Auth.VerifyUser;

public class VerifyUserByAuthenticatorQuery : IQuery<VerifyUserResponseDto>
{
    public string AuthenticatorToken { get; set; }
    public string UrlToken { get; set; }

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