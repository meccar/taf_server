using Microsoft.Extensions.DependencyInjection;

namespace Application.Configurations.Api;

public static class CorsConfiguration
{
    public static IServiceCollection ConfigureCors(this IServiceCollection services, string appCors)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(appCors, policy =>
                {
                    policy
                        .WithOrigins("https://localhost:7293")
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
        return services;
    }
}