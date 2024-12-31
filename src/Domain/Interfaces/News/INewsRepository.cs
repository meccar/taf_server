using Domain.Aggregates;
using Shared.Model;
using Shared.Results;

namespace Domain.Interfaces.News;

public interface INewsRepository
{
    Task<List<NewsAggregate>?> GetAllNewsAsync();
    Task<NewsAggregate?> CreateNewsAsync(NewsAggregate newsAggregate);
    Task<NewsAggregate?> GetDetailNewsAsync(string id);
    Task<NewsAggregate?> UpdateNewsAsync(NewsAggregate newsAggregate);
    Task<NewsAggregate?> SoftDeleteAsync(NewsAggregate newsAggregate);
}