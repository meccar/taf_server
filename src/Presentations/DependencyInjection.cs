using Presentations.Configurations;

namespace Presentations;

/// <summary>
/// Provides methods to configure and register services for the Presentation layer.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Configures the services required by the Presentation layer, including controller services.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> used to register services.</param>
    /// <param name="configurations">The configuration settings used to configure services.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> with the Presentation layer services configured.</returns>
    public static IServiceCollection ConfigurePresentationsServices(
        this IServiceCollection services,
        IConfiguration configurations)
    {
        // services.ConfigureHttpException();
        services.ConfigureControllers();

        return services;
    }
}