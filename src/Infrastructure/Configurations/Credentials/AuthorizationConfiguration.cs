using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Shared.Configurations.Environment;
using Shared.Enums;
using Shared.Policies;

namespace Infrastructure.Configurations.Credentials;

public static class AuthorizationConfiguration
{
    public static IServiceCollection ConfigureAuthorization(this IServiceCollection services, EnvironmentConfiguration configuration)
    {
        services
            .AddAuthorization(options =>
                {
                    ConfigureRolePolicy(options, FORole.Admin);
                    ConfigureRolePolicy(options, FORole.CompanyManager);
                    ConfigureRolePolicy(options, FORole.CompanyUser);
                    ConfigureRolePolicy(options, FORole.User);
                    
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