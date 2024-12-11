using System.Text.Json;
using System.Text.Json.Serialization;
using Presentations.Controllers.Exceptions;

namespace Presentations.Configurations;

/// <summary>
/// Provides extension methods for configuring controllers and their behavior.
/// </summary>
public static class ControllersConfiguration
{
    /// <summary>
    /// Configures controllers with custom JSON serialization settings and exception filters.
    /// </summary>
    /// <param name="services">The service collection to add the configurations to.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance with the configured controllers.</returns>
    public static IServiceCollection ConfigureControllers(this IServiceCollection services)
    {
        services
            .AddControllers(options =>
            {
                options.Filters.Add<ExceptionsController>();
            })
            .AddJsonOptions(options =>
            {
                // options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                options.JsonSerializerOptions.MaxDepth = 64;
            });
            // .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
                
        return services;
    }
}