using Serilog;
using taf_server.Infrastructure;
using taf_server.Presentations.Extensions;

var AppCors = "AppCors";

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

Log.Information("Starting EvenHub API up");

try
{
    // builder.Host.UseSerilog(LoggingConfiguration.Configure);
    builder.Host.AddAppConfigurations();

    // Add services to the container.
    builder.Logging.AddConsole();
    // builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddGrpc();
    builder.Services.ConfigureInfrastructureServices(builder.Configuration, AppCors);

    WebApplication? app = builder.Build();

    app.UseInfrastructure(AppCors);

    app.Run();

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
    Log.CloseAndFlush();
}