using Domain.Aggregates;
using Shared.Model;
using Shared.Results;

namespace Domain.Interfaces.News;

public interface INewsRepository
{
    Task<Result<List<NewsAggregate>>> GetAllNewsAsync();
    Task<Result<NewsModel>> CreateNewsAsync(NewsModel newsModel);
    Task<Result<NewsAggregate>> GetDetailNewsAsync(string id);
    Task<Result<NewsModel>> UpdateAsync(NewsModel newsModel);
}