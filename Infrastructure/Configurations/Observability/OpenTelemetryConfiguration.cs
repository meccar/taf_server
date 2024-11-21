using Duende.IdentityServer;
using Infrastructure.Configurations.Environment;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry.Metrics;

namespace Infrastructure.Configurations.Observability;

public static class OpenTelemetryConfiguration
{
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