using Domain.SeedWork.Query;
using Shared.Dtos.Authentication.Credentials;
using Shared.Model;

namespace Application.Queries.Auth.VerifyUser;

public class VerifyUserQuery : IQuery<VerifyUserRequestDto>
{
    public VerifyUserRequestDto Token { get; set; }
    public VerifyUserQuery(VerifyUserRequestDto token) =>
        Token = token;
}