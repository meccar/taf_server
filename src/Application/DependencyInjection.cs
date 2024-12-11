using Application.Configurations;
using Application.Configurations.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

/// <summary>
/// Provides extension methods for configuring the application's dependency injection container.
/// This class is responsible for wiring up various services and configurations needed for the application.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Configures and registers the necessary services for the application.
    /// This method wires up dependencies such as CORS, API versioning, Swagger, AutoMapper, validation, and MediatR.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> that is used to register the application's services.</param>
    /// <param name="configurations">The <see cref="IConfiguration"/> instance used to access configuration settings for the application.</param>
    /// <param name="appCors">A string representing the CORS policy name to be applied.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> with the application's dependencies configured.</returns>
    public static IServiceCollection ConfigureApplicationDependencyInjection(
        this IServiceCollection services,
        IConfiguration configurations,
        string appCors
        )
    {
        // Configure CORS policy
        services.ConfigureCors(appCors);
        
        // Configure API versioning and related settings
        services.ConfigureApi();
        
        // Configure Swagger for API documentation
        services.ConfigureSwagger();
        
        // Configure AutoMapper for object mappings
        services.ConfigureMapper();
        
        // Configure FluentValidation validators
        services.ConfigureValidatiors();
        
        // Configure MediatR for handling requests
        services.ConfigureMediatR();
        
        return services;
    }
}