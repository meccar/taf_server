using Infrastructure.Abstractions;
using Infrastructure.Data;
using Infrastructure.SeedWork.SqlConnection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configurations.Infrastructure;

public static class DependencyInjectionConfiguration
{
    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services)
    {
        services
            .AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>()
            .AddScoped<ApplicationDbContextSeed>();
            
        return services;
    }
}