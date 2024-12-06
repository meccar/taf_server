using Application.Commands.Auth.Register;
using Application.Queries.Auth.Login;
using Infrastructure.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Configurations;

/// <summary>
/// Provides configuration for setting up MediatR in the application.
/// This class registers MediatR handlers, behaviors, and other services necessary for handling 
/// application commands, queries, and pipeline behaviors.
/// </summary>
public static class MediatRConfiguration
{
    /// <summary>
    /// Configures and registers MediatR services in the application.
    /// This method sets up the necessary handlers for commands and queries, 
    /// as well as pipeline behaviors for exception handling, logging, and performance monitoring.
    /// </summary>
    /// <param name="services">The collection of services for dependency injection.</param>
    /// <returns>The <see cref="IServiceCollection"/> with MediatR configured.</returns>
    /// <remarks>
    /// This method registers the MediatR handlers from the assemblies of the `RegisterCommandHandler` 
    /// and `LoginQueryHandler`. Additionally, it adds pipeline behaviors for exception handling, logging,
    /// and performance monitoring to intercept requests and responses throughout the MediatR pipeline.
    /// </remarks>
    public static IServiceCollection ConfigureMediatR(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            // Registers command and query handlers from the RegisterCommandHandler and LoginQueryHandler assemblies.
            config.RegisterServicesFromAssembly(typeof(RegisterCommandHandler).Assembly);
            config.RegisterServicesFromAssembly(typeof(LoginQueryHandler).Assembly);
            
            // Registers custom pipeline behaviors for exception handling, logging, and performance monitoring.
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ExceptionBehavior<,>));
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviors<,>));
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
        });
            
        return services;
    }
    
}