using Asp.Versioning;
using Asp.Versioning.Conventions;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Configurations.Api;

/// <summary>
/// Provides configuration for setting up API versioning and conventions in the application.
/// This class is used to configure API versioning, including default versions, versioning strategies, and versioning conventions.
/// </summary>
public static class ApiConfiguration
{
    /// <summary>
    /// Configures API versioning and MVC conventions for the application.
    /// Sets up default API version, versioning strategy (via URL segment and custom header), 
    /// and integrates versioning conventions for API documentation and routing.
    /// </summary>
    /// <param name="services">The collection of services for dependency injection.</param>
    /// <returns>The <see cref="IServiceCollection"/> with API versioning and MVC configured.</returns>
    public static IServiceCollection ConfigureApi(this IServiceCollection services)
    {
        services
            // Configure API versioning
            .AddApiVersioning(options =>
            {
                // The default API version to be used if no version is specified in the request.
                options.DefaultApiVersion = new ApiVersion(1, 0);
                
                // When true, assumes the default API version when no version is specified in the request.
                options.AssumeDefaultVersionWhenUnspecified = true;
                
                // Enables the reporting of API versions in the response headers.
                options.ReportApiVersions = true;
                
                // Configures how the API version is read from the request.
                // This combines versioning from URL segments and custom header (`X-Api-Version`).
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-Api-Version")
                );
            })
            // Configure MVC and add versioning conventions
            .AddMvc(
                options =>
                {
                    // Adds a versioning convention based on namespaces.
                    options.Conventions.Add( new VersionByNamespaceConvention() );
                })
            // Configure API Explorer for documentation
            .AddApiExplorer(
                options =>
                {
                    // Specifies the format for version group names in the API documentation.
                    options.GroupNameFormat = "'v'VVV";
                    
                    // Substitutes the API version in the URL for generated documentation.
                    options.SubstituteApiVersionInUrl = true;
                });
                
        // services.AddHttpContextAccessor();
        
        return services;
    }
}