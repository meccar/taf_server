using taf_server.Presentations.HttpResponse;

namespace taf_server.Infrastructure.Configurations;

public static class ExceptionsConfiguration
{
    public static IServiceCollection ConfigureExceptions(this IServiceCollection services)
    {
        services.AddScoped<HttpExceptionFilter>();
        
        return services;
    }
}