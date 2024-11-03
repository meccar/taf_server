using Application.Dtos.Authentication.Register;
using Application.Validators.Auth;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Configurations;

public static class ValidatorsConfiguration
{
    public static IServiceCollection ConfigureValidatiors(this IServiceCollection services)
    {
         services
             .AddScoped<IValidator<RegisterUserRequestDto>, RegisterValidator>();
         
         return services;
    }
}