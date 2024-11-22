using System.Security.Cryptography.X509Certificates;
using Application;
using IdentityModel.Client;
using IdentityServer.Extensions;
using Infrastructure;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using Serilog;

namespace IdentityServer;

public static class HostingExtensions
{
    private static readonly string _appCors = "AppCors";
    
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        var client = new HttpClient();
        client.GetDiscoveryDocumentAsync("https://localhost:6001");
        
        app.UseApplicationSetup(_appCors);
        
        return app;
    }

    public static WebApplication ConfigureBuilder(this WebApplicationBuilder builder)
    {
        var serviceName = "taf_server";
        var serviceVersion = "1.0.0";
        
        var kestrelConfig = builder.Configuration.GetSection("Kestrel:Endpoints:Https:Certificate");

        var cert = new X509Certificate2(kestrelConfig["Path"]!, kestrelConfig["Password"]);
        
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();
        builder.Services.ConfigureInfrastructureServices(builder.Configuration, _appCors);
        builder.Services.ConfigureApplicationServices(builder.Configuration);
        
        builder.Logging.AddConsole();
        builder.Logging.AddSerilog();
        builder.Logging.AddOpenTelemetry(options => options
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(
                serviceName: serviceName,
                serviceVersion: serviceVersion))
            .AddConsoleExporter());
    
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