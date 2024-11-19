using System.Security.Cryptography.X509Certificates;
using Application;
using IdentityModel.Client;
using IdentityServer.Extensions;
using Infrastructure;
using Serilog;

var AppCors = "AppCors";

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
var cert = new X509Certificate2("../certificate.pfx", "tung");

try
{
    builder.Logging.AddConsole();
    
    // builder.Host.UseSerilog((ctx, lc) => lc
    //     .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
    //     .Enrich.FromLogContext()
    //     .ReadFrom.Configuration(ctx.Configuration));
    
    // Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services.AddRazorPages();
    builder.Services.ConfigureInfrastructureServices(builder.Configuration, AppCors);
    builder.Services.ConfigureApplicationServices(builder.Configuration);
    
    builder.WebHost.ConfigureKestrel(options =>
    {
        // Configure Kestrel to use your .pfx certificate for HTTPS
        options.ConfigureHttpsDefaults(httpsOptions =>
        {
            httpsOptions.ServerCertificate = cert;
        });
    });
    
    // builder.Services.ConfigurePresentationsServices(builder.Configuration);

    var client = new HttpClient();
    await client.GetDiscoveryDocumentAsync("https://localhost:6001");
    
    WebApplication? app = builder.Build();

    app.UseInfrastructure(AppCors);

    await app.RunAsync();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, $"Unhandled exception: {ex.Message}");

    var type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;
}
finally
{
    Log.Information("Shut down API complete");
    await Log.CloseAndFlushAsync();
}