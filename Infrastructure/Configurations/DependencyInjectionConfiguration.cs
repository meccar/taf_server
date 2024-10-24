using taf_server.Infrastructure.Abstractions;
using taf_server.Infrastructure.Data;
using taf_server.Infrastructure.SeedWork.SqlConnection;

namespace taf_server.Infrastructure.Configurations;

public static class DependencyInjectionConfiguration
{
    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services)
    {
        services
            .AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
            // .AddTransient<ApplicationDbContextSeed>();
            
        return services;
    }
}