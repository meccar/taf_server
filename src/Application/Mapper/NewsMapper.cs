using AutoMapper;
using Domain.Aggregates;
using Shared.Dtos.News;
using Shared.Model;

namespace Application.Mapper;

public static class NewsMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<CreateNewsRequestDto, NewsModel>();
        config.CreateMap<NewsModel, CreateNewsResponseDto>();
        config.CreateMap<NewsModel, GetAllNewsResponseDto>();
        // config.CreateMap<List<NewsModel>, List<GetAllNewsResponseDto>>();
        config.CreateMap<NewsModel, GetDetailNewsResponseDto>();
        config.CreateMap<NewsModel, NewsAggregate>();
        config.CreateMap<NewsAggregate, NewsModel>();
        
        config.CreateMap<NewsAggregate, GetDetailNewsResponseDto>();

        
    }
}