using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry.Metrics;

namespace Infrastructure.Configurations.Observability;

/// <summary>
/// Provides extension methods to configure OpenTelemetry for observability in the application.
/// </summary>
public static class OpenTelemetryConfiguration
{
    /// <summary>
    /// Configures OpenTelemetry services for tracing and metrics in the application.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add OpenTelemetry services to.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection ConfigureOpenTelemetry(this IServiceCollection services)
    {
        var serviceName = "taf_server";
        var serviceVersion = "1.0.0";
        
        services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(
                serviceName: serviceName,
                serviceVersion: serviceVersion))
            .WithTracing(tracing => tracing
                .AddSource(serviceName)
                .AddAspNetCoreInstrumentation()
                .AddConsoleExporter())
            .WithMetrics(metrics => metrics
                .AddMeter(serviceName)
                .AddConsoleExporter());
        
        return services;
    }
}