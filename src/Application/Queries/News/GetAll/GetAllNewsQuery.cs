using Domain.SeedWork.Query;
using Shared.Dtos.News;
using Shared.Dtos.Pagination;

namespace Application.Queries.News.GetAll;

public class GetAllNewsQuery : IQuery<PaginationResponse<GetAllNewsResponseDto>>
{
    public PaginationParams PaginationParams { get; set; } 
    public GetAllNewsQuery(
        PaginationParams paginationParams
    ) => PaginationParams = paginationParams;
    
}