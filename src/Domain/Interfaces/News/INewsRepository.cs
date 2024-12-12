using Domain.Aggregates;
using Shared.Model;
using Shared.Results;

namespace Domain.Interfaces.News;

public interface INewsRepository
{
    Task<Result<List<NewsModel>>> GetAllNewsAsync();
    Task<Result<NewsModel>> CreateNewsAsync(NewsModel newsModel);
    Task<Result<NewsModel>> GetDetailNewsAsync(string id);
}