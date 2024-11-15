using Infrastructure.SeedWork.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configurations.Identity;

public static class AuthenticationConfiguration
{
    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services)
    {
        services
            .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
                    options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
                    options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
                }
            )
            .AddBearerToken(IdentityConstants.BearerScheme, options =>
            {
                options.ClaimsIssuer = "test";
            });
        
        return services;
    }
}