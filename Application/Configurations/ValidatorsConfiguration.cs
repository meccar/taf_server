using System.Reflection;
using Application.Dtos.Authentication.Register;
using Application.Validators.Auth;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Configurations;

public static class ValidatorsConfiguration
{
    public static IServiceCollection ConfigureValidatiors(this IServiceCollection services)
    {
         services
             .AddScoped<IValidator<RegisterUserRequestDto>, RegisterValidator>()
             .AddFluentValidationAutoValidation()
             .AddFluentValidationClientsideAdapters()
             .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
             .AddFluentValidationRulesToSwagger();;
         
         return services;
    }
}