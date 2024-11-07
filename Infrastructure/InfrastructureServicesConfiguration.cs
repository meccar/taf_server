using Infrastructure.Configurations.Api;
using Infrastructure.Configurations.Database;
using Infrastructure.Configurations.Identity;
using Infrastructure.Configurations.Infrastructure;
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
        services.ConfigureDependencyInjection();
        services.ConfigureCors(appCors);
        // services.ConfigureTransaction();
        services.ConfigureApiVersioning();
        services.ConfigureDbContext(configuration);
        services.ConfigureSwagger();
        services.ConfigureIdentity();
        services.ConfigureRepositories();

        return services;
    }
}