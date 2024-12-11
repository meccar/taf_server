namespace IdentityServer.Pipeline;

/// <summary>
/// Provides extension methods for configuring the host in the application.
/// </summary>
public static class HostExtensions
{
    /// <summary>
    /// Adds application-specific configurations to the host's configuration system.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance to configure.</param>
    public static void AddAppConfigurations(this WebApplicationBuilder builder)
    {
        var env = builder.Environment;
        builder.Configuration
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
            .AddEnvironmentVariables();
    }
}