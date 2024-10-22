using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;

namespace taf_server.Infrastructure.Configurations;

public static class ValidatorConfiguration
{
    public static IServiceCollection ConfigureValidation(this IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddFluentValidationRulesToSwagger();
        
        return services;
    }
}