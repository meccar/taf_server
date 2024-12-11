using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Dtos.Authentication.Register;
using Shared.Validators.Auth;

namespace Application.Configurations;

/// <summary>
/// Provides configuration for setting up FluentValidation and Swagger integration in the application.
/// This class registers validators for the application's data transfer objects (DTOs) and configures 
/// automatic validation for API requests.
/// </summary>
public static class ValidatorsConfiguration
{
    /// <summary>
    /// Configures FluentValidation services for the application.
    /// Registers validators for the data transfer objects (DTOs), enables automatic validation
    /// for incoming requests, and integrates validation rules with Swagger documentation.
    /// </summary>
    /// <param name="services">The collection of services for dependency injection.</param>
    /// <returns>The <see cref="IServiceCollection"/> with FluentValidation configured.</returns>
    /// <remarks>
    /// This method registers the <see cref="RegisterValidator"/> for the <see cref="RegisterUserRequestDto"/> type, 
    /// enabling automatic validation on API requests. Additionally, it configures the integration of validation rules 
    /// into Swagger documentation, making it easier for API consumers to understand the validation rules associated with 
    /// the request models.
    /// </remarks>
    public static IServiceCollection ConfigureValidatiors(this IServiceCollection services)
    {
         services
             // Register the custom validator for RegisterUserRequestDto
             .AddScoped<IValidator<RegisterUserRequestDto>, RegisterValidator>()
             
             // Enable automatic validation for API requests
             .AddFluentValidationAutoValidation()

             // Enable client-side validation support for Swagger UI
             .AddFluentValidationClientsideAdapters()
             
             // Register all validators from the current assembly
             .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
             
             // Integrate validation rules into Swagger for better API documentation
             .AddFluentValidationRulesToSwagger();
         
         return services;
    }
}