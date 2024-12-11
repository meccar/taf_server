namespace Presentations.Extensions;

/// <summary>
/// Provides extension methods for configuring the host builder.
/// </summary>
public static class HostExtensions
{
    /// <summary>
    /// Adds application-specific configurations to the <see cref="WebApplicationBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance to configure.</param>
    public static void AddAppConfigurations(this WebApplicationBuilder builder)
    {
        var env = builder.Environment;

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    }
}