using System.Text.Json;
using System.Text.Json.Serialization;

namespace IdentityServer.Configurations;

/// <summary>
/// Provides configuration for ASP.NET Core controllers.
/// </summary>
/// <remarks>
/// This class configures settings for controllers, including JSON serialization settings, 
/// controller behavior, and options for model state validation.
/// </remarks>
public static class ControllersConfiguration
{
    /// <summary>
    /// Configures ASP.NET Core controllers with custom options for JSON serialization,
    /// API behavior, and model validation.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> used to register services.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> with configured controller options.</returns>
    /// <remarks>
    /// This method sets up the following controller settings:
    /// <list type="bullet">
    ///     <item><description>Suppresses implicit required attributes for non-nullable reference types.</description></item>
    ///     <item><description>Configures JSON serialization options such as enum conversion, number handling, and property naming policy.</description></item>
    ///     <item><description>Sets a maximum depth for JSON serialization to prevent excessive nesting.</description></item>
    ///     <item><description>Configures API behavior to suppress model state invalid filters, allowing custom validation handling.</description></item>
    /// </list>
    /// </remarks>
    public static IServiceCollection ConfigureControllers(this IServiceCollection services)
    {
        services
            .AddControllers(options =>
            {
                // Suppresses the implicit requirement for non-nullable reference types
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            })
            .AddJsonOptions(options =>
            {
                // Adds a converter for enum types to use string values during serialization
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

                // Allows named floating point literals (e.g., "NaN" or "Infinity") in JSON numbers
                options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;

                // Ignores null values when writing JSON
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

                // Uses camelCase for JSON property naming (e.g., "propertyName" instead of "PropertyName")
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

                // Preserves object references during JSON serialization
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;

                // Sets the maximum depth allowed for JSON serialization to prevent excessive recursion
                options.JsonSerializerOptions.MaxDepth = 32;
            })
            .ConfigureApiBehaviorOptions(options =>
                // Suppresses the default invalid model state response from the framework
                options.SuppressModelStateInvalidFilter = true
            );

        return services;
    }
}