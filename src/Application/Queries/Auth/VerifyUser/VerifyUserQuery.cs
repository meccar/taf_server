using Domain.SeedWork.Query;
using Shared.Model;

namespace Application.Queries.Auth.VerifyUser;

public class VerifyUserQuery : IQuery<TokenModel>
{
    public string Token { get; set; }
    public VerifyUserQuery(string token) =>
        Token = token;
}