using Application.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureApplicationServices(
        this IServiceCollection services,
        IConfiguration configurations)
    {
        services.ConfigureSwagger();
        services.ConfigureMapper();
        services.ConfigureValidatiors();
        services.ConfigureUsecases();
        services.ConfigureMediatR();
        
        return services;
    }
}