using Domain.Abstractions;
using Domain.Aggregates;

namespace Domain.Interfaces.News;

public interface INewsRepository : IRepositoryBase<NewsAggregate>
{
}