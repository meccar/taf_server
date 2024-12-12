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

    public async Task<Result<List<NewsModel>>> GetAllNewsAsync()
    {
        var result =  FindAll(true)
                                            .ToList();
        
        return result != null
            ? Result<List<NewsModel>>.Success(_mapper.Map<List<NewsModel>>(result))
            : Result<List<NewsModel>>.Failure("Failed to get news");
    }
    
    public async Task<Result<NewsModel>> GetDetailNewsAsync(string id)
    {
        var result = await FindByCondition(x => x.Uuid == id, true)
            .FirstOrDefaultAsync();
        
        return result != null
            ? Result<NewsModel>.Success(_mapper.Map<NewsModel>(result))
            : Result<NewsModel>.Failure("Failed to get news");
    }
    
    public async Task<Result<NewsModel>> CreateNewsAsync(NewsModel newsModel)
    {
        var newsAggregate = _mapper.Map<NewsAggregate>(newsModel);
        
        var created = await CreateAsync(newsAggregate);

        return created != null
            ? Result<NewsModel>.Success(_mapper.Map<NewsModel>(created.Entity))
            : Result<NewsModel>.Failure("Failed to create news");
    }
}