using Application.Mapper;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Configurations;

/// <summary>
/// Provides configuration for setting up AutoMapper in the application.
/// This class is responsible for registering AutoMapper profiles and mappings to facilitate 
/// object-to-object mapping between different data models.
/// </summary>
public static class MapperConfiguration
{
    /// <summary>
    /// Configures and registers AutoMapper profiles for the application.
    /// This method sets up the necessary object-to-object mappings using AutoMapper by 
    /// creating mappings for various domain models, such as user account, login data, and tokens.
    /// </summary>
    /// <param name="services">The collection of services for dependency injection.</param>
    /// <returns>The <see cref="IServiceCollection"/> with AutoMapper configured.</returns>
    /// <remarks>
    /// This method registers the necessary AutoMapper profiles to convert between different types 
    /// used in the application. Mappings include user account details, login data, and token information.
    /// </remarks>
    public static IServiceCollection ConfigureMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(config =>
        {
            // Registers mappings for UserAccount
            UserAccountMapper.CreateMap(config);
            
            // Registers mappings for UserLoginData
            UserLoginDataMapper.CreateMap(config);
            
            // Registers mappings for UserToken
            UserTokenMapper.CreateMap(config);
            
            // Registers mappings for Auth-related models
            AuthMapper.CreateMap(config);
            
            // Registers mappings for News
            NewsMapper.CreateMap(config);
        });
        
        return services;
    }

}