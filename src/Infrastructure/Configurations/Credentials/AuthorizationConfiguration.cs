using Infrastructure.Configurations.Environment;
using Infrastructure.SeedWork.Enums;
using Infrastructure.SeedWork.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configurations.Credentials;

public static class AuthorizationConfiguration
{
    public static IServiceCollection ConfigureAuthorization(this IServiceCollection services, EnvironmentConfiguration configuration)
    {
        services
            .AddAuthorization(options =>
                {
                    ConfigureRolePolicy(options, ERole.Admin);
                    ConfigureRolePolicy(options, ERole.CompanyManager);
                    ConfigureRolePolicy(options, ERole.CompanyUser);
                    ConfigureRolePolicy(options, ERole.User);
                    
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