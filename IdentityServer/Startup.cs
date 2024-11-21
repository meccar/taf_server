using System.Security.Cryptography.X509Certificates;
using Application;
using IdentityModel.Client;
using IdentityServer.Extensions;
using Infrastructure;
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
        Logging.AddConsole();
        Logging.AddSerilog();
    }
    
    public void ConfigureWebHost(IWebHostBuilder WebHost)
    {
        var cert = new X509Certificate2("../certificate.pfx", "tung");

        WebHost.ConfigureKestrel(options =>
        {
            options.ConfigureHttpsDefaults(httpsOptions =>
            {
                httpsOptions.ServerCertificate = cert;
            });
        });
    }
}