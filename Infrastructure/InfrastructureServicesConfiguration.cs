using taf_server.Infrastructure.Configurations;

namespace taf_server.Infrastructure;

public static class InfrastructureServicesConfiguration
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration, string appCors)
    {
        // services.ConfigureRedis(configuration);
        // services.ConfigureCors(appCors);
        // services.ConfigureQuartz();
        // services.ConfigureHangfireServices();
        // services.ConfigureApplication();
        // services.ConfigureAppSettings(configuration);
        // services.ConfigureAuthetication();
        services.ConfigureDependencyInjection();
        services.ConfigureMediatR();
        services.ConfigureSwagger();
        services.ConfigureDbContext(configuration);
        services.ConfigureIdentity();
        services.ConfigureMapper();
        services.ConfigureControllers();
        services.ConfigureApiVersioning();
        services.ConfigureValidation();

        return services;
    }
}