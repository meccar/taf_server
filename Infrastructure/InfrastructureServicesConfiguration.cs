using Infrastructure.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureServicesConfiguration
{
    public static IServiceCollection ConfigureInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration,
        string appCors)
    {
        // services.ConfigureRedis(configuration);
        // services.ConfigureQuartz();
        // services.ConfigureHangfireServices();
        // services.ConfigureApplication();
        // services.ConfigureAppSettings(configuration);
        // services.ConfigureAuthetication();
        // services.ConfigureDependencyInjection();
        services.ConfigureCors(appCors);
        services.ConfigureApiVersioning();
        services.ConfigureDbContext(configuration);
        services.ConfigureSwagger();
        services.ConfigureValidation();
        services.ConfigureIdentity();
        services.ConfigureRepositories();
        services.ConfigureControllers();

        return services;
    }
}