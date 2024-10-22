using Asp.Versioning;
using Asp.Versioning.Conventions;

namespace taf_server.Infrastructure.Configurations;

public static class ControllersConfiguration
{
    public static IServiceCollection ConfigureControllers(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);
            // .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
                
        return services;
    }
}