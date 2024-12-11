using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Shared.Model;

namespace Application.Mapper;

/// <summary>
/// Provides the mapping configurations for user token models and Identity framework token models.
/// This class configures the mapping between the <see cref="UserTokenModel"/> and the <see cref="IdentityUserToken{TKey}"/> for transforming
/// token data between the application's models and the Identity framework's token storage.
/// </summary>
public static class UserTokenMapper
{
    /// <summary>
    /// Configures the AutoMapper mappings for user token models and Identity framework token models.
    /// This method sets up the mapping between <see cref="UserTokenModel"/> and <see cref="int"/>,
    /// </summary>
    /// <param name="config">The AutoMapper configuration expression used to create the mappings.</param>
    /// <remarks>
    /// This method ensures that token data can be seamlessly transferred between the <see cref="UserTokenModel"/> used in the
    /// application and the <see cref="IdentityUserToken{TKey}"/> used by ASP.NET Identity. It also customizes the mapping for certain
    /// </remarks>
    public static void CreateMap(IMapperConfigurationExpression config)
    {
        // Map UserTokenModel to IdentityUserToken<int>
        config.CreateMap<UserTokenModel, IdentityUserToken<int>>();
        
        // Map IdentityUserToken<int> to UserTokenModel, specifying custom mappings for certain properties
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