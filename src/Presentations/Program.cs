using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Presentations;

/// <summary>
/// The entry point for the application, configuring the application services and running the host.
/// </summary>
public class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    /// <param name="args">Command line arguments passed to the application.</param>
    /// <returns>A task representing the asynchronous operation. Returns 0 if successful, 1 if an error occurred.</returns>
    public static async Task<int> Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
            .CreateLogger();
        
        try
        {
            Log.Information("Starting host...");
            
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            
            WebApplication app = builder
                .ConfigureBuilder()
                .ConfigurePipeline();

            Log.Information("Seeding database...");
            await SeedData.EnsureSeedData(app);
            Log.Information("Done seeding database. Exiting.");
            
            await app.RunAsync();

            return 0;
        }
        catch (Exception ex) when (ex is not HostAbortedException)
        {
            Log.Fatal(ex, $"Unhandled exception: {ex.Message}");

            var type = ex.GetType().Name;
            if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;
            return 1;
        }
        finally
        {
            Log.Information("Shut down API complete");
            await Log.CloseAndFlushAsync();
        }
    }
}