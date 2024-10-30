using Application.Dtos.Authentication.Register;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Presentations.Validators.Auth;

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