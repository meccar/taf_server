using DataBase.Configurations;
using Infrastructure.Configurations.Api;
using Infrastructure.Configurations.Credentials;
using Infrastructure.Configurations.Identity;
using Infrastructure.Configurations.Infrastructure;
using Infrastructure.Configurations.Observability;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Share.Configurations.Environment;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureInfrastructureDependencyInjection(
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

        services.ConfigureDbContext(config);
        services.ConfigureCors(appCors);
        services.ConfigureApi();
        services.ConfigureOpenTelemetry();
        services.ConfigureAuthentication(config);
        services.ConfigureAuthorization(config);
        // services.ConfigureTransaction();
        // services.ConfigureSwagger();
        services.ConfigureIdentity();
        // services.ConfigureIdentityServer(config);
        services.ConfigureRepositories();

        return services;
    }
}