using System.Text.Json;
using System.Text.Json.Serialization;

namespace taf_server.Infrastructure.Configurations;

public static class ControllersConfiguration
{
    public static IServiceCollection ConfigureControllers(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                // options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });
            // .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
                
        return services;
    }
}