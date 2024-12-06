using System.Security.Cryptography.X509Certificates;
using Application;
using IdentityModel.Client;
using IdentityServer.Pipeline;
using Infrastructure;
using Infrastructure.Configurations.IdentityServer;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using Serilog;
using Shared.Configurations.Environment;

namespace IdentityServer;

/// <summary>
/// Provides extension methods for configuring and hosting the IdentityServer application.
/// </summary>
public static class Hosting
{
    private static readonly string AppCorsPolicy = "AppCors";

    /// <summary>
    /// Configures the HTTP request pipeline for the application.
    /// </summary>
    /// <param name="app">The WebApplication instance to configure.</param>
    /// <returns>The configured WebApplication instance.</returns>
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        ConfigureDiscoveryClient();

        app.UseApplicationSetup(AppCorsPolicy);

        return app;
    }

    /// <summary>
    /// Configures the services and application settings for the IdentityServer application.
    /// </summary>
    /// <param name="builder">The WebApplicationBuilder instance used for configuration.</param>
    /// <returns>The built WebApplication instance.</returns>
    public static WebApplication ConfigureBuilder(this WebApplicationBuilder builder)
    {
        var serviceName = "taf_server";
        var serviceVersion = "1.0.0";

        // Load certificate for HTTPS configuration
        var cert = LoadCertificate(builder.Configuration);

        var config = new EnvironmentConfiguration(builder.Configuration);

        // Configure services
        ConfigureServices(builder, config);

        // Configure logging
        ConfigureLogging(builder, serviceName, serviceVersion);

        // Configure Kestrel server with certificate
        ConfigureKestrel(builder, cert);

        return builder.Build();
    }

    // Loads the certificate from configuration
    private static X509Certificate2 LoadCertificate(IConfiguration configuration)
    {
        var kestrelConfig = configuration.GetSection("Kestrel:Endpoints:Https:Certificate");
        return new X509Certificate2(kestrelConfig["Path"]!, kestrelConfig["Password"]);
    }

    // Adds services to the DI container
    private static void ConfigureServices(WebApplicationBuilder builder, EnvironmentConfiguration config)
    {
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();
        builder.Services.AddBff();
        builder.Services.ConfigureInfrastructureDependencyInjection(builder.Configuration);
        builder.Services.ConfigureIdentityServer(config);
        builder.Services.ConfigureIdentityServerAuthentication(config);
        builder.Services.ConfigureApplicationDependencyInjection(builder.Configuration, AppCorsPolicy);
    }

    // Configures logging with Console, Serilog, and OpenTelemetry
    private static void ConfigureLogging(WebApplicationBuilder builder, string serviceName, string serviceVersion)
    {
        builder.Logging.AddConsole();
        builder.Logging.AddSerilog();
        builder.Logging.AddOpenTelemetry(options =>
            options.SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService(serviceName, serviceVersion))
                .AddConsoleExporter());
    }

    // Configures Kestrel server for HTTPS using a certificate
    private static void ConfigureKestrel(WebApplicationBuilder builder, X509Certificate2 cert)
    {
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ConfigureHttpsDefaults(httpsOptions =>
            {
                httpsOptions.ServerCertificate = cert;
            });
        });
    }

    // Initializes and sends a discovery request
    private static void ConfigureDiscoveryClient()
    {
        var client = new HttpClient();
        client.GetDiscoveryDocumentAsync("https://localhost:6001");
    }
}

