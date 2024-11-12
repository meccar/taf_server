using Application.Dtos.Authentication.Register;
using Application.Dtos.UserAccount;
using AutoMapper;
using Domain.Aggregates;
using Domain.Entities;
using Domain.Model;
using Domain.SeedWork.Enums.Token;
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
        config.CreateMap<UserTokenModel, UserTokenEntity>();
        config.CreateMap<UserTokenEntity, UserTokenModel>();
        config.CreateMap<IdentityUserToken<Guid>, UserTokenModel>()
            .ForMember(dest => 
                dest.UserAccountId,
                opt => 
                    opt.MapFrom(src => 
                        src.UserId.ToString()))
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