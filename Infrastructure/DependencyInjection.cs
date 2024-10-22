
using MediatR;
using taf_server.Infrastructure.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace taf_server.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration, string appCors)
    {
        // services.ConfigureRedis(configuration);
        services.ConfigureControllers();
        services.ConfigureApiVersioning();
        // services.ConfigureCors(appCors);
        // services.ConfigureApplicationDbContext(configuration);
        // services.ConfigureQuartz();
        // services.ConfigureHangfireServices();
        // services.ConfigureMediatR();
        // services.ConfigureIdentity();
        // services.ConfigureMapper();
        // services.ConfigureSwagger();
        // services.ConfigureAppSettings(configuration);
        // services.ConfigureApplication();
        // services.ConfigureAuthetication();
        services.ConfigureValidation();
        services.ConfigureDependencyInjection();

        return services;
    }

    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services)
    {
        // services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandler<>));

        // services.AddScoped<IProcessOutboxMessagesJob, ProcessOutboxMessagesJob>();

        // services
        //     .AddTransient<ISerializeService, SerializeService>()
        //     .AddTransient<ICacheService, CacheService>()
        //     .AddTransient<IFileService, AzureFileService>()
        //     .AddTransient<IHangfireService, HangfireService>()
        //     .AddTransient<IEmailService, EmailService>()
        //     .AddTransient<ITokenService, TokenService>()
        //     .AddTransient<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}