using Domain.Aggregates;
using Domain.Interfaces.News;
using Microsoft.EntityFrameworkCore;
using Persistance.Data;

namespace Persistance.Repositories.News;

public class NewsRepository
    : RepositoryBase<NewsAggregate>, INewsRepository 
{
    public NewsRepository(
        ApplicationDbContext context
    ) : base(context)
    {
    }

    public async Task<List<NewsAggregate>?> GetAllNewsAsync()
    {
        var result =  FindAll(true)
                                            .ToList();
        
        return result.Count > 0 ? result : null;
    }
    
    public async Task<NewsAggregate?> GetDetailNewsAsync(string id)
    {
        return await FindByCondition(x => x.Uuid == id, true)
                                            .FirstOrDefaultAsync();
    }
    
    public async Task<NewsAggregate?> CreateNewsAsync(NewsAggregate newsAggregate)
    {
        var created = await CreateAsync(newsAggregate);

        return created?.Entity;
    }

    public async Task<NewsAggregate?> UpdateNewsAsync(NewsAggregate newsAggregate)
    {
        return await UpdateAsync(newsAggregate);
    }

    public async Task<NewsAggregate?> SoftDeleteAsync(NewsAggregate newsAggregate)
    {
        return await UpdateAsync(newsAggregate);
    }
}