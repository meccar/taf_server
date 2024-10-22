
using FluentValidation;
using MediatR;
using taf_server.Infrastructure.Configurations;
using Microsoft.Extensions.DependencyInjection;
using taf_server.Domain.Interfaces;
using taf_server.Infrastructure.Repositories;
using taf_server.Infrastructure.Repositories.Command;
using taf_server.Presentations.Dtos.Authentication;
using taf_server.Presentations.Validators.Auth;

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
        services.ConfigureDbContext(configuration);
        // services.ConfigureQuartz();
        // services.ConfigureHangfireServices();
        // services.ConfigureMediatR();
        // services.ConfigureIdentity();
        services.ConfigureMapper();
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
        
        services.AddScoped<IUserAccountCommandRepository, UserAccountCommandRepository>();
        services.AddScoped<IUserLoginDataCommandRepository, UserLoginDataCommandRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<IValidator<RegisterUserRequestDto>, RegisterValidator>();
        
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