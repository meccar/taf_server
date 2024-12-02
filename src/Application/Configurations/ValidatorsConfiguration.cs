using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Dtos.Authentication.Register;
using Shared.Validators.Auth;

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
             .AddFluentValidationRulesToSwagger();
         
         return services;
    }
}