using Application.Configurations;
using Application.Configurations.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureApplicationDependencyInjection(
        this IServiceCollection services,
        IConfiguration configurations,
        string appCors
        )
    {
        services.ConfigureCors(appCors);
        services.ConfigureApi();
        services.ConfigureSwagger();
        services.ConfigureMapper();
        services.ConfigureValidatiors();
        services.ConfigureMediatR();
        
        return services;
    }
}