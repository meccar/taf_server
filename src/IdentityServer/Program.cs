using System.Diagnostics;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace IdentityServer;

/// <summary>
/// The main entry point for the IdentityServer application.
/// Configures and runs the web application.
/// </summary>
public class Program
{
    /// <summary>
    /// The entry point for the application. Configures and runs the web application.
    /// </summary>
    /// <param name="args">The command-line arguments passed to the application.</param>
    /// <returns>Returns an integer exit code indicating success (0) or failure (1).</returns>
    public static async Task<int> Main(string[] args)
    {
        Activity.DefaultIdFormat = ActivityIdFormat.W3C;
        
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
            
            WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
            
            WebApplication? app = builder
                .ConfigureBuilder()
                .ConfigurePipeline();
            
            // if (args.Contains("/seed"))
            // {
            //     Log.Information("Seeding database...");
            //     SeedData.EnsureSeedData(app);
            //     Log.Information("Done seeding database. Exiting.");
            //     return;
            // }
            
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
            Log.Information("Shut down complete");
            await Log.CloseAndFlushAsync();
        }
    }
}