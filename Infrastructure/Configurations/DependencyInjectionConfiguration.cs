using Infrastructure.Abstractions;
using Infrastructure.SeedWork.SqlConnection;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configurations;

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