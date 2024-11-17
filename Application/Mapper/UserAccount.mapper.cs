using AutoMapper;
using Domain.Aggregates;
using Domain.Model;

namespace Application.Mapper;

public static class UserAccountMapper
{
    /// <summary>
    /// Configures the mappings for user account models and entities.
    /// </summary>
    /// <param name="config">The AutoMapper configuration expression used to create the mappings.</param>
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<UserAccountAggregate, UserAccountModel>()
            .ForMember(dest => dest.UserLoginData, opt => opt.MapFrom(src => src.UserLoginData));
        config.CreateMap<UserAccountModel, UserAccountAggregate>()
            .ForMember(dest => dest.EId, opt => opt.Ignore());
    }
}