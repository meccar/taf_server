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
    public NewsRepository(
        ApplicationDbContext context
    ) : base(context)
    {
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
        
        return result is not null
            ? Result<NewsAggregate>.Success(result)
            : Result<NewsAggregate>.Failure("The News does not exist");
    }
    
    public async Task<Result<NewsAggregate>> CreateNewsAsync(NewsAggregate newsAggregate)
    {
        var created = await CreateAsync(newsAggregate);

        return created is not null
            ? Result<NewsAggregate>.Success(created.Entity)
            : Result<NewsAggregate>.Failure("Failed to create news");
    }

    public async Task<Result<NewsAggregate>> UpdateNewsAsync(NewsAggregate newsAggregate)
    {
        var result = await UpdateAsync(newsAggregate);
        
        return result is not null
            ? Result<NewsAggregate>.Success(result)
            : Result<NewsAggregate>.Failure("Failed to update news");
    }

    public async Task<Result<NewsAggregate>> SoftDeleteAsync(NewsAggregate newsAggregate)
    {
        var updated = await UpdateAsync(newsAggregate);
        
        return updated is not null
            ? Result<NewsAggregate>.Success(updated)
            : Result<NewsAggregate>.Failure("Failed to update news");
    }
}