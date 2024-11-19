using AutoMapper;
using Domain.Model;
using Microsoft.AspNetCore.Identity;

namespace Application.Mapper;

public static class UserTokenMapper
{
    /// <summary>
    /// Configures the mappings for user account models and entities.
    /// </summary>
    /// <param name="config">The AutoMapper configuration expression used to create the mappings.</param>
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        config.CreateMap<UserTokenModel, IdentityUserToken<int>>();
        config.CreateMap<IdentityUserToken<int>, UserTokenModel>()
            .ForMember(dest => 
                dest.Name, 
                opt => 
                    opt.MapFrom(src => 
                        src.Name))
            .ForMember(dest => 
                dest.Value, 
                opt => 
                    opt.MapFrom(src => 
                        src.Value))
            .ForMember(dest => 
                dest.LoginProvider, 
                opt => 
                    opt.MapFrom(src => 
                        src.LoginProvider));
    }
}