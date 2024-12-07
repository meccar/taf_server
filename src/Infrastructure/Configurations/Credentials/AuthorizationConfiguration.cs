using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Shared.Configurations.Environment;
using Shared.FileObjects;
using Shared.Policies;

namespace Infrastructure.Configurations.Credentials;

/// <summary>
/// Provides extension methods for configuring authorization policies in the application.
/// </summary>
public static class AuthorizationConfiguration
{
    /// <summary>
    /// Configures authorization policies for the application, including role-based policies.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the authorization configuration to.</param>
    /// <param name="configuration">The environment configuration containing necessary settings.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> with authorization services configured.</returns>
    public static IServiceCollection ConfigureAuthorization(this IServiceCollection services, EnvironmentConfiguration configuration)
    {
        services
            .AddAuthorization(options =>
                {
                    ConfigureRolePolicy(options, FoRole.Admin);
                    ConfigureRolePolicy(options, FoRole.CompanyManager);
                    ConfigureRolePolicy(options, FoRole.CompanyUser);
                    ConfigureRolePolicy(options, FoRole.User);
                    
                    // options.AddPolicy("ApiScope", policy =>
                    // {
                    //     policy.RequireAuthenticatedUser();
                    //     // policy.RequireClaim("client_id", configuration.GetIdentityServerInteractiveClientId());
                    //     policy.RequireClaim("scope", IdentityServerConstants.StandardScopes.OpenId);
                    // });
                }
            );
        return services;
    }
    
    private static void ConfigureRolePolicy(AuthorizationOptions options, string role)
    {
        options
            .AddPolicy(new PolicyKey(role).ToString(),
                policy => policy.RequireRole(role));
    }
}