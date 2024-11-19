using Application;
using Infrastructure;
using Infrastructure.Data;
using Presentations;
using Presentations.Extensions;
using Serilog;

var AppCors = "AppCors";

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

try
{
    // builder.Host.UseSerilog(LoggingConfiguration.Configure);
    builder.Host.AddAppConfigurations();

    // Add services to the container.
    builder.Logging.AddConsole();
    
    builder.Services.ConfigureInfrastructureServices(builder.Configuration, AppCors);
    builder.Services.ConfigureApplicationServices(builder.Configuration);
    builder.Services.ConfigurePresentationsServices(builder.Configuration);
    
    WebApplication? app = builder.Build();

    app.UseInfrastructure(AppCors);
    
    await SeedDatabaseAsync(app);
    
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

async Task SeedDatabaseAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var serviceProvider = scope.ServiceProvider;
    var dbContextSeed = serviceProvider.GetRequiredService<ApplicationDbContextSeed>();
    await dbContextSeed.SeedAsync();
}