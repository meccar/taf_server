using Infrastructure.Configurations.Credentials;
using Infrastructure.Configurations.DataBase;
using Infrastructure.Configurations.Identity;
using Infrastructure.Configurations.Observability;
using Infrastructure.Configurations.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Configurations.Environment;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureInfrastructureDependencyInjection(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var config = new EnvironmentConfiguration(configuration);
        
        // services.ConfigureRedis(configuration);
        // services.ConfigureQuartz();
        // services.ConfigureHangfireServices();
        // services.ConfigureApplication();
        // services.ConfigureAppSettings(configuration);

        services.ConfigureDbContext(config);
        services.ConfigureOpenTelemetry();
        services.ConfigureAuthentication(config);
        services.ConfigureAuthorization(config);
        services.ConfigureIdentity();
        services.ConfigureRepositories();

        return services;
    }
}