using System.Diagnostics;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using IdentityServer;

public class Program
{
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
            
            var builder = WebApplication.CreateBuilder(args);
            var startup = new Startup(builder.Configuration);
            
            startup.ConfigureServices(builder.Services);
            startup.ConfigureLogging(builder.Logging);
            startup.ConfigureWebHost(builder.WebHost);
            
            var app = builder.Build();
            startup.ConfigureApplication(app);
            
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