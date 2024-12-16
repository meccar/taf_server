using Domain.Aggregates;
using Shared.Model;
using Shared.Results;

namespace Domain.Interfaces.News;

public interface INewsRepository
{
    Task<Result<List<NewsAggregate>>> GetAllNewsAsync();
    Task<Result<NewsAggregate>> CreateNewsAsync(NewsAggregate newsAggregate);
    Task<Result<NewsAggregate>> GetDetailNewsAsync(string id);
    Task<Result<NewsAggregate>> UpdateAsync(NewsAggregate newsAggregate);
}