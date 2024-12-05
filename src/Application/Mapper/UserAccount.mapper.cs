using AutoMapper;
using Domain.Aggregates;
using Shared.Model;

namespace Application.Mapper;

public static class UserAccountMapper
{
    /// <summary>
    /// Configures the mappings for user account models and entities.
    /// </summary>
    /// <param name="config">The AutoMapper configuration expression used to create the mappings.</param>
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<UserProfileAggregate, UserProfileModel>()
            .ForMember(dest => dest.UserAccount, opt => opt.MapFrom(src => src.UserAccount));
        config.CreateMap<UserProfileModel, UserProfileAggregate>()
            .ForMember(dest => dest.EId, opt => opt.Ignore());
    }
}