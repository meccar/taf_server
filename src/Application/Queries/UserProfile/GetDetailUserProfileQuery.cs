using Domain.SeedWork.Query;
using Shared.Dtos.UserProfile;

namespace Application.Queries.UserProfile;

public class GetDetailUserProfileQuery : IQuery<GetDetailUserProfileResponseDto>
{
    public string Eid { get; set; }
    public GetDetailUserProfileQuery(
        string eid
    ) => Eid = eid;
}