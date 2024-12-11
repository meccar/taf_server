using Microsoft.Extensions.DependencyInjection;

namespace Application.Configurations.Api;

/// <summary>
/// Provides configuration for setting up Cross-Origin Resource Sharing (CORS) in the application.
/// This class is used to configure CORS policies, allowing specific origins, headers, and methods for cross-origin requests.
/// </summary>
public static class CorsConfiguration
{
    /// <summary>
    /// Configures the CORS (Cross-Origin Resource Sharing) policy for the application.
    /// This method sets up a policy to allow requests from specific origins and allows credentials, headers, and methods.
    /// </summary>
    /// <param name="services">The collection of services for dependency injection.</param>
    /// <param name="appCors">The name of the CORS policy to be applied.</param>
    /// <returns>The <see cref="IServiceCollection"/> with CORS configured.</returns>
    /// <remarks>
    /// The policy created by this method allows CORS requests from the specified origin 
    /// (https://localhost:7293), and it enables credentials, any headers, and any HTTP methods.
    /// This configuration is typically used for development environments or specific scenarios 
    /// where frontend applications make cross-origin requests.
    /// </remarks>
    public static IServiceCollection ConfigureCors(this IServiceCollection services, string appCors)
    {
        services.AddCors(options =>
        {
            // Adds a CORS policy with the specified name
            options.AddPolicy(appCors, policy =>
                {
                    // Allow requests from a specific origin (localhost:7293)
                    policy
                        .WithOrigins("https://localhost:7293")
                        
                        // Allow credentials (cookies, authorization headers, etc.)
                        .AllowCredentials()
                        
                        // Allow any header to be sent with the request
                        .AllowAnyHeader()
                        
                        // Allow any HTTP method (GET, POST, PUT, DELETE, etc.)
                        .AllowAnyMethod();
                });
        });
        return services;
    }
}