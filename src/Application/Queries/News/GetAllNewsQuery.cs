using Domain.SeedWork.Query;
using Shared.Dtos.News;
using Shared.Dtos.Pagination;

namespace Application.Queries.News;

public class GetAllNewsQuery : IQuery<PaginationResponse<GetAllNewsResponseDto>>
{
    public PaginationParams PaginationParams { get; set; } 
    public GetAllNewsQuery(
        PaginationParams paginationParams
    ) => PaginationParams = paginationParams;
    
}