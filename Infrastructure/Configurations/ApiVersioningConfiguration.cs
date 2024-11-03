using Asp.Versioning;
using Asp.Versioning.Conventions;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configurations;

public static class ApiVersioningConfiguration
{
    public static IServiceCollection ConfigureApiVersioning(this IServiceCollection services)
    {
        services
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-Api-Version")
                );
            })
            .AddMvc(
                options =>
                {
                    options.Conventions.Add( new VersionByNamespaceConvention() );
                })
            .AddApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });
                
        return services;
    }
}