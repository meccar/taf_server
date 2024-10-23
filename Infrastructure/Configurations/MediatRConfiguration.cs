using MediatR;
using taf_server.Application.Behaviors;

namespace taf_server.Infrastructure.Configurations;

public static class MediatRConfiguration
{
    public static IServiceCollection ConfigureMediatR(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly);
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ExceptionBehavior<,>));
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviors<,>));
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
        });
            
        return services;
    }
    
}