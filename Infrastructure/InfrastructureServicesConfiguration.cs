using Infrastructure.Configurations.Api;
using Infrastructure.Configurations.Database;
using Infrastructure.Configurations.Environment;
using Infrastructure.Configurations.Identity;
using Infrastructure.Configurations.IdentityServer;
using Infrastructure.Configurations.Infrastructure;
using Infrastructure.Configurations.Observability;
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
        var config = new EnvironmentConfiguration(configuration);
        
        // services.ConfigureRedis(configuration);
        // services.ConfigureQuartz();
        // services.ConfigureHangfireServices();
        // services.ConfigureApplication();
        // services.ConfigureAppSettings(configuration);
        services.ConfigureDependencyInjection();
        services.ConfigureCors(appCors);
        services.ConfigureOpenTelemetry();
        services.ConfigureAuthentication(config);
        services.ConfigureAuthorization();
        // services.ConfigureTransaction();
        services.ConfigureApiVersioning();
        services.ConfigureDbContext(config);
        // services.ConfigureSwagger();
        services.ConfigureIdentity();
        services.ConfigureIdentityServer(config);
        services.ConfigureRepositories();

        return services;
    }
}