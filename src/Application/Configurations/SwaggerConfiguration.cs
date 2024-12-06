using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Shared.Dtos.Authentication.Login;
using Swashbuckle.AspNetCore.Swagger;

namespace Application.Configurations;

/// <summary>
/// Provides configuration for setting up Swagger in the application.
/// This class configures Swagger for API documentation generation, 
/// including versioning, metadata, and XML comment inclusion.
/// </summary>
public static class SwaggerConfiguration
{
    /// <summary>
    /// Configures Swagger services in the application.
    /// This method sets up Swagger documentation for the API, including basic API info, 
    /// versioning, and XML comment inclusion to generate API documentation from code comments.
    /// </summary>
    /// <param name="services">The collection of services for dependency injection.</param>
    /// <returns>The <see cref="IServiceCollection"/> with Swagger configured.</returns>
    /// <remarks>
    /// This method configures Swagger to display API version "v1" with metadata such as the API title, 
    /// description, and license. Additionally, it includes XML comments for API models to enhance the documentation 
    /// with descriptions from code comments. This is useful for auto-generating detailed API docs for consumers.
    /// </remarks>
    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services
            // Configure Swagger options to serialize Swagger document as v2
            .Configure<SwaggerOptions>(c => c.SerializeAsV2 = true)
            
            // Add endpoints for API explorer
            .AddEndpointsApiExplorer()
            
            // Add Swagger generation services
            .AddSwaggerGen(options =>
            {
                // Define Swagger document with version v1 and API metadata
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Taf API",
                    Description = "Taf API",
                    TermsOfService = new Uri("https://www.taf.io"),
                    Contact = new OpenApiContact
                    {
                        Name = "Taf",
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                    }
                });
                
                // options.ExampleFilters();
                //
                // options.OperationFilter<AddHeaderOperationFilter>("correlationId", "Correlation Id for the request", false);
                // options.OperationFilter<AddResponseHeadersFilter>();
                
                var xmlFilename = Path.Combine(AppContext.BaseDirectory, "..", "..", "Shared");
                
                // Path to XML comments for API models (used for enhancing Swagger docs)
                var sharedXmlPath = Path.Combine(Path.GetDirectoryName(typeof(LoginUserRequestDto).Assembly.Location)!, "Shared.xml");

                // Include XML comments in the Swagger documentation
                options.IncludeXmlComments(sharedXmlPath);
                
                // options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
            });
        
        return services;
    }
}