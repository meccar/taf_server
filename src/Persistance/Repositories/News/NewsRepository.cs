using AutoMapper;
using Domain.Aggregates;
using Domain.Interfaces.News;
using Microsoft.EntityFrameworkCore;
using Persistance.Data;
using Shared.Model;
using Shared.Results;

namespace Persistance.Repositories.News;

public class NewsRepository
    : RepositoryBase<NewsAggregate>, INewsRepository 
{
    private readonly IMapper _mapper;

    public NewsRepository(
        ApplicationDbContext context,
        IMapper mapper
    ) : base(context)
    {
        _mapper = mapper;
    }

    public async Task<Result<List<NewsAggregate>>> GetAllNewsAsync()
    {
        var result =  FindAll(true)
                                            .ToList();
        
        return result.Count > 0
            ? Result<List<NewsAggregate>>.Success(result)
            : Result<List<NewsAggregate>>.Failure("Failed to get news");
    }
    
    public async Task<Result<NewsAggregate>> GetDetailNewsAsync(string id)
    {
        var result = await FindByCondition(x => x.Uuid == id, true)
                                            .FirstOrDefaultAsync();
        
        return result != null
            ? Result<NewsAggregate>.Success(result)
            : Result<NewsAggregate>.Failure("Failed to get news");
    }
    
    public async Task<Result<NewsAggregate>> CreateNewsAsync(NewsAggregate newsAggregate)
    {
        var created = await CreateAsync(newsAggregate);

        return created != null
            ? Result<NewsAggregate>.Success(created.Entity)
            : Result<NewsAggregate>.Failure("Failed to create news");
    }

    public async Task<Result<NewsAggregate>> UpdateAsync(NewsAggregate newsAggregate)
    {
        var result = await UpdateAsync(newsAggregate);
        
        return result.Succeeded
            ? Result<NewsAggregate>.Success(result.Value!)
            : Result<NewsAggregate>.Failure(result.Errors.FirstOrDefault() ?? "Failed to update news");
    }
}