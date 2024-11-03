using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configurations;

public static class CorsConfiguration
{
    public static IServiceCollection ConfigureCors(this IServiceCollection services, string appCors)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(appCors, policy =>
                {
                    policy
                        // .WithOrigins("*")
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
        return services;
    }
}