using Domain.Aggregates;
using Domain.Interfaces.News;
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
}