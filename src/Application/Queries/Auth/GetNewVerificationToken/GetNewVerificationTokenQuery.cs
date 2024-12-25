using Domain.SeedWork.Query;
using Shared.Dtos;

namespace Application.Queries.Auth.GetNewVerificationToken;

public class GetNewVerificationTokenQuery : IQuery<SuccessResponseDto>
{
    public string UserAccountEid { get; set; }
    public GetNewVerificationTokenQuery(
        string eid
    ) => UserAccountEid = eid;
}