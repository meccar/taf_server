using Infrastructure.SeedWork.Enums;
using Infrastructure.SeedWork.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configurations.Identity;

public static class AuthorizationConfiguration
{
    public static IServiceCollection ConfigureAuthorization(this IServiceCollection services)
    {
        services
            .AddAuthorization(options =>
                {
                    ConfigureRolePolicy(options, ERole.Admin);
                    ConfigureRolePolicy(options, ERole.CompanyManager);
                    ConfigureRolePolicy(options, ERole.CompanyUser);
                    ConfigureRolePolicy(options, ERole.User);
                    
                    options.AddPolicy("ApiScope", policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.RequireClaim("scope", "api1");
                    });
                }
            );
        return services;
    }
    
    private static void ConfigureRolePolicy(AuthorizationOptions options, string role)
    {
        options.AddPolicy(new PolicyKey(role).ToString(), policy =>
            policy.RequireRole(role));
    }
}