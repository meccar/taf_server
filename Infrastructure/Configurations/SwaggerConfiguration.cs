using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Infrastructure.Configurations;

public static class SwaggerConfiguration
{
    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Taf API",
                    Description = "Taf API",
                    TermsOfService = new Uri("https://www.taf.io"),
                    Contact = new OpenApiContact
                    {
                        Name = "Taf",
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                    }
                });
                
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        
        return services;
    }
}