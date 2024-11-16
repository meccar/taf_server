using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;

namespace Application.Configurations;

public static class SwaggerConfiguration
{
    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services
            .Configure<SwaggerOptions>(c => c.SerializeAsV2 = true)
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
                
                options.ExampleFilters();

                options.OperationFilter<AddHeaderOperationFilter>("correlationId", "Correlation Id for the request", false);
                options.OperationFilter<AddResponseHeadersFilter>();
                
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
            });
        
        return services;
    }
}