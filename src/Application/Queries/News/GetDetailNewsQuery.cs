using Domain.SeedWork.Query;
using Shared.Dtos.News;

namespace Application.Queries.News;

public class GetDetailNewsQuery : IQuery<GetDetailNewsResponseDto>
{
    public string Eid { get; set; }

    public GetDetailNewsQuery(
        string eid
    ) => Eid = eid;
}