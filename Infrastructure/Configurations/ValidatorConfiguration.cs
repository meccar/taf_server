using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using taf_server.Presentations.Dtos.Authentication.Register;
using taf_server.Presentations.Validators.Auth;

namespace taf_server.Infrastructure.Configurations;

public static class ValidatorConfiguration
{
    public static IServiceCollection ConfigureValidation(this IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddFluentValidationRulesToSwagger()
            .AddScoped<IValidator<RegisterUserRequestDto>, RegisterValidator>();

        return services;
    }
}