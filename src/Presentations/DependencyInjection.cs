using Presentations.Configurations;

namespace Presentations;

public static class DependencyInjection
{
    public static IServiceCollection ConfigurePresentationsServices(
        this IServiceCollection services,
        IConfiguration configurations)
    {
        // services.ConfigureHttpException();
        services.ConfigureControllers();

        return services;
    }
}