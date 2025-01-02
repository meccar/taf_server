using AutoMapper;
using Domain.Aggregates;
using Domain.Interfaces;
using Shared.Dtos.Exceptions;
using Shared.Dtos.News;
using Shared.Dtos.Pagination;
using Shared.Helpers;

namespace Application.Queries.News.GetAll;

public class GetAllNewsCommandHandler : TransactionalQueryHandler<GetAllNewsQuery, PaginationResponse<GetAllNewsResponseDto>>
{
    private readonly IMapper _mapper;

    public GetAllNewsCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    protected override async Task<PaginationResponse<GetAllNewsResponseDto>> ExecuteCoreAsync(GetAllNewsQuery request, CancellationToken cancellationToken)
    {
        var newsAggregate = UnitOfWork
                                .NewsRepository
                                .FindAll(true)
                                .ToList();

        if (newsAggregate.Count < 1)
            throw new NotFoundException("No News found");

        var paginationResponse = new PaginationHelper<NewsAggregate>(
            pageNumber: request.PaginationParams.PageNumber,
            pageSize: request.PaginationParams.PageSize,
            searchTerm: request.PaginationParams.SearchTerm!,
            sortBy: request.PaginationParams.SortBy!,
            isAscending: request.PaginationParams.IsAscending,
            category: request.PaginationParams.Category!
        );
    
        var paginatedNews = paginationResponse.Paginate(newsAggregate);
        
        return new PaginationResponse<GetAllNewsResponseDto>
        {
            Items = _mapper.Map<List<GetAllNewsResponseDto>>(paginatedNews.Items),
            PageNumber = paginatedNews.PageNumber,
            PageSize = paginatedNews.PageSize,
            TotalItems = paginatedNews.TotalItems,
            TotalPages = paginatedNews.TotalPages,
            HasPreviousPage = paginatedNews.HasPreviousPage,
            HasNextPage = paginatedNews.HasNextPage
        };
    }
}