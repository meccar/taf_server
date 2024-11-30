using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configurations.Infrastructure;

public static class DependencyInjectionConfiguration
{
    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services)
    {
        services
            .AddScoped<ApplicationDbContextSeed>()
            .AddHttpContextAccessor();
            
        return services;
    }
}