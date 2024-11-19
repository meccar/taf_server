using Application;
using IdentityModel.Client;
using IdentityServer.Areas.Extensions;
using Infrastructure;
using Serilog;

var AppCors = "AppCors";

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);



try
{
    builder.Logging.AddConsole();
    
    // builder.Host.UseSerilog((ctx, lc) => lc
    //     .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
    //     .Enrich.FromLogContext()
    //     .ReadFrom.Configuration(ctx.Configuration));
    
    // Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services.ConfigureInfrastructureServices(builder.Configuration, AppCors);
    builder.Services.ConfigureApplicationServices(builder.Configuration);
    
    builder.Services.AddRazorPages();
    // builder.Services.ConfigurePresentationsServices(builder.Configuration);

    var client = new HttpClient();
    await client.GetDiscoveryDocumentAsync("https://localhost:5001");
    
    WebApplication? app = builder.Build();

    app.UseInfrastructure(AppCors);

    await app.RunAsync();
}
catch (Exception ex)
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