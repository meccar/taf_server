using Application.Commands.Auth.Register;
using Application.Queries.Auth.Login;
using Infrastructure.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Configurations;

/// <summary>
/// 
/// </summary>
public static class MediatRConfiguration
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureMediatR(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(RegisterCommandHandler).Assembly);
            config.RegisterServicesFromAssembly(typeof(LoginQueryHandler).Assembly);
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ExceptionBehavior<,>));
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviors<,>));
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
        });
            
        return services;
    }
    
}