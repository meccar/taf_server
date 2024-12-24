using Application.Commands.News;
using Application.Commands.News.Create;
using AutoMapper;
using Domain.Aggregates;
using Shared.Dtos.News;

namespace Application.Mapper;

public static class NewsMapper
{
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<CreateNewsCommand, NewsAggregate>();
        config.CreateMap<NewsAggregate, GetDetailNewsResponseDto>();

        
    }
}