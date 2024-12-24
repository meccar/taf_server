using Domain.SeedWork.Query;
using Shared.Dtos.News;

namespace Application.Queries.News.GetDetail;

public class GetDetailNewsQuery : IQuery<GetDetailNewsResponseDto>
{
    public string Eid { get; set; }

    public GetDetailNewsQuery(
        string eid
    ) => Eid = eid;
}