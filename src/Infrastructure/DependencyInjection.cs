using Infrastructure.Configurations.Credentials;
using Infrastructure.Configurations.DataBase;
using Infrastructure.Configurations.Identity;
using Infrastructure.Configurations.Observability;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Configurations.Repositories;
using Shared.Configurations.Environment;

namespace Infrastructure;

/// <summary>
/// Provides extension methods for configuring infrastructure dependencies in the service container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Configures infrastructure services including database, authentication, identity, observability, and repositories.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">The application's configuration to use for settings.</param>
    /// <returns>The configured <see cref="IServiceCollection"/> instance.</returns>
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
        // services.ConfigureRepositories();

        return services;
    }
}