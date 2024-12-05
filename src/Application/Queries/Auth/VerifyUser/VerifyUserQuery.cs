using Domain.SeedWork.Query;
using Shared.Dtos.Authentication.Credentials;
using Shared.Model;

namespace Application.Queries.Auth.VerifyUser;

public class VerifyUserQuery : IQuery<VerifyUserEmailRequestDto>
{
    public VerifyUserEmailRequestDto Token { get; set; }
    public VerifyUserQuery(VerifyUserEmailRequestDto token) =>
        Token = token;
}