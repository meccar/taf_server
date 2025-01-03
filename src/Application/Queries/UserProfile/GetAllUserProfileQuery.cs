using Domain.SeedWork.Query;
using Shared.Dtos.Pagination;
using Shared.Dtos.UserProfile;

namespace Application.Queries.UserProfile;

public class GetAllUserProfileQuery : IQuery<PaginationResponse<GetAllUserProfileResponseDto>>
{
    public PaginationParams PaginationParams { get; set; }
    public GetAllUserProfileQuery(
        PaginationParams paginationParams
    ) => PaginationParams = paginationParams;
}