using Application.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationServicesConfiguration
{
    public static IServiceCollection ConfigureApplicationServices(
        this IServiceCollection services,
        IConfiguration configurations)
    {
        services.ConfigureMapper();
        services.ConfigureValidatiors();
        services.ConfigureUsecases();
        services.ConfigureMediatR();

        return services;
    }
}