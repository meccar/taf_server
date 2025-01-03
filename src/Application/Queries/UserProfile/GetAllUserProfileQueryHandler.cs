using AutoMapper;
using Domain.Aggregates;
using Domain.Interfaces;
using Shared.Dtos.Exceptions;
using Shared.Dtos.Pagination;
using Shared.Dtos.UserProfile;
using Shared.Helpers;

namespace Application.Queries.UserProfile;

public class GetAllUserProfileQueryHandler : TransactionalQueryHandler<GetAllUserProfileQuery, PaginationResponse<GetAllUserProfileResponseDto>>
{
    private readonly IMapper _mapper;
    
    public GetAllUserProfileQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    protected override async Task<PaginationResponse<GetAllUserProfileResponseDto>> ExecuteCoreAsync(GetAllUserProfileQuery request, CancellationToken cancellationToken)
    {
        var userProfileAggregate = UnitOfWork
            .UserProfileRepository.FindAll(true).ToList();
        
        if (userProfileAggregate.Count < 1)
            throw new NotFoundException("No user found");

        var paginationResponse = new PaginationHelper<UserProfileAggregate>(
            pageNumber: request.PaginationParams.PageNumber,
            pageSize: request.PaginationParams.PageSize,
            searchTerm: request.PaginationParams.SearchTerm!,
            sortBy: request.PaginationParams.SortBy!,
            isAscending: request.PaginationParams.IsAscending,
            category: request.PaginationParams.Category!
        );
        
        var paginatedUserProfile = paginationResponse.Paginate(userProfileAggregate);

        return new PaginationResponse<GetAllUserProfileResponseDto>
        {
            Items = _mapper.Map<List<GetAllUserProfileResponseDto>>(paginatedUserProfile.Items),
            PageNumber = paginatedUserProfile.PageNumber,
            PageSize = paginatedUserProfile.PageSize,
            TotalItems = paginatedUserProfile.TotalItems,
            TotalPages = paginatedUserProfile.TotalPages,
            HasPreviousPage = paginatedUserProfile.HasPreviousPage,
            HasNextPage = paginatedUserProfile.HasNextPage
        };
    }
}