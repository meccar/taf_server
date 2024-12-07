using System.Net;
using System.Security.Cryptography.X509Certificates;
using Application;
using Infrastructure;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using Presentations.Configurations;
using Presentations.Extensions;
using Serilog;

namespace Presentations;

/// <summary>
/// Provides extension methods for configuring and setting up the application hosting pipeline.
/// </summary>
public static class HostingExtensions
{
    private static readonly string AppCors = "AllowLocalhost";
    
    /// <summary>
    /// Configures the HTTP request pipeline for the application by calling the ApplicationSetup method.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> to configure.</param>
    /// <returns>The configured <see cref="WebApplication"/>.</returns>
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.ApplicationSetup(AppCors);
        
        return app;
    }
    
    /// <summary>
    /// Configures the application builder by setting up services, logging, Kestrel, and other configurations.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> to configure.</param>
    /// <returns>The configured <see cref="WebApplication"/>.</returns>
    public static WebApplication ConfigureBuilder(this WebApplicationBuilder builder)
    {
        var serviceName = "taf_server";
        var serviceVersion = "1.0.0";
        
        var kestrelConfig = builder.Configuration.GetSection("Kestrel:Endpoints:Https:Certificate");

        var cert = new X509Certificate2(kestrelConfig["Path"]!, kestrelConfig["Password"]);
        
        builder.Services.ConfigureInfrastructureDependencyInjection(builder.Configuration);
        builder.Services.ConfigureApplicationDependencyInjection(builder.Configuration, AppCors);
        // builder.Services.ConfigurePresentationsServices(builder.Configuration);
        // builder.Services.ConfigureHttpException();
        builder.Services.ConfigureControllers();
        
        builder.Logging.AddConsole();
        builder.Logging.AddSerilog();
        builder.Logging.AddOpenTelemetry(options => options
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(
                serviceName: serviceName,
                serviceVersion: serviceVersion))
            .AddConsoleExporter());
    
        builder.AddAppConfigurations();
        
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ConfigureHttpsDefaults(httpsOptions =>
            {
                httpsOptions.ServerCertificate = cert;
                httpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13;
            });
            
            options.Listen(IPAddress.Any, 7293, listenOptions =>
            {
                listenOptions.UseHttps(kestrelConfig["Path"]!, kestrelConfig["Password"]);
            });
        });
        
        return builder.Build();
    }
}