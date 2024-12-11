using System.Text;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared.Configurations.Environment;

namespace Infrastructure.Configurations.Credentials;

/// <summary>
/// Provides extension methods for configuring authentication in the application.
/// </summary>
public static class AuthenticationConfiguration
{
    /// <summary>
    /// Configures authentication services for the application.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add authentication services to.</param>
    /// <param name="configuration">The environment-specific configuration.</param>
    /// <returns>The <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, EnvironmentConfiguration configuration)
    {
        services
            .AddAuthentication(options =>
                {
                    // options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
                    options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
                    // options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    // options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                    // options.DefaultAuthenticateScheme = OpenIdConnectDefaults.AuthenticationScheme;
                    // options.DefaultSignInScheme = OpenIdConnectDefaults.AuthenticationScheme;
                    options.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
                    options.RequireAuthenticatedSignIn = true;
                }
            )
            .AddJwtBearer(configuration.GetJwtType()!, options =>
            {
                options.Authority = configuration.GetIdentityServerAuthority();
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.Email,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetJwtSecret()!)),
                    ValidateAudience = true,
                    ValidateIssuer = false,
                    ValidIssuer = configuration.GetIdentityServerAuthority(),
                    ValidAudience = configuration.GetIdentityServerInteractiveClientId(),
                    ClockSkew = TimeSpan.Zero,
                };
            })
            .AddCookie(configuration.GetJwtRefreshCookieKey()!, options =>
            {

                // host prefixed cookie name
                options.Cookie.Name = configuration.GetJwtRefreshCookieKey();

                // strict SameSite handling
                options.Cookie.SameSite = SameSiteMode.Strict;

                // set session lifetime
                options.ExpireTimeSpan = TimeSpan.FromHours(8);

                // sliding or absolute
                options.SlidingExpiration = false;
            });
        
        return services;
    }
}