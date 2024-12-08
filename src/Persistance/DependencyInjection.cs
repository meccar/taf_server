using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Configurations.Repositories;

namespace Persistance;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureDataBaseDependencyInjection(
        this IServiceCollection services,
        IConfiguration configuration
        )
    {
        // var config = new EnvironmentConfiguration(configuration);

        services.ConfigureRepositories();

        return services;
    }
}