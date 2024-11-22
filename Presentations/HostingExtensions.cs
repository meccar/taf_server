using System.Security.Cryptography.X509Certificates;
using Application;
using Infrastructure;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using Presentations.Extensions;
using Serilog;

namespace Presentations;

public static class HostingExtensions
{
    private static readonly string _appCors = "AppCors";
    
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.ApplicationSetup(_appCors);
        
        return app;
    }
    
    public static WebApplication ConfigureBuilder(this WebApplicationBuilder builder)
    {
        var serviceName = "taf_server";
        var serviceVersion = "1.0.0";
        
        var kestrelConfig = builder.Configuration.GetSection("Kestrel:Endpoints:Https:Certificate");

        var cert = new X509Certificate2(kestrelConfig["Path"]!, kestrelConfig["Password"]);
        
        builder.Services.ConfigureInfrastructureServices(builder.Configuration, _appCors);
        builder.Services.ConfigureApplicationServices(builder.Configuration);
        builder.Services.ConfigurePresentationsServices(builder.Configuration);
        
        builder.Logging.AddConsole();
        builder.Logging.AddSerilog();
        builder.Logging.AddOpenTelemetry(options => options
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(
                serviceName: serviceName,
                serviceVersion: serviceVersion))
            .AddConsoleExporter());
    
        builder.Host.AddAppConfigurations();
        
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ConfigureHttpsDefaults(httpsOptions =>
            {
                httpsOptions.ServerCertificate = cert;
            });
        });
        
        return builder.Build();
    }
}