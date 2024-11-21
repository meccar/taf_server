using System.Security.Cryptography.X509Certificates;
using Application;
using IdentityModel.Client;
using IdentityServer.Extensions;
using Infrastructure;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using Serilog;

namespace IdentityServer;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly string _appCors = "AppCors";
    
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void ConfigureApplication(WebApplication app)
    {
        var client = new HttpClient();
        client.GetDiscoveryDocumentAsync("https://localhost:6001");
        
        app.UseApplicationSetup(_appCors);
    }

    public void ConfigureServices(IServiceCollection Services)
    {
        Services.AddControllersWithViews();
        Services.AddRazorPages();
        Services.ConfigureInfrastructureServices(_configuration, _appCors);
        Services.ConfigureApplicationServices(_configuration);
    }
    
    public void ConfigureLogging(ILoggingBuilder Logging)
    {
        var serviceName = "taf_server";
        var serviceVersion = "1.0.0";
        
        Logging.AddConsole();
        Logging.AddSerilog();
        Logging.AddOpenTelemetry(options => options
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(
                serviceName: serviceName,
                serviceVersion: serviceVersion))
            .AddConsoleExporter());
    }
    
    // public void ConfigureWebHost(IWebHostBuilder WebHost)
    // {
    //     var cert = new X509Certificate2("../certificate.pfx", "tung");
    //
    //     WebHost.ConfigureKestrel(options =>
    //     {
    //         options.ConfigureHttpsDefaults(httpsOptions =>
    //         {
    //             httpsOptions.ServerCertificate = cert;
    //         });
    //     });
    // }
}