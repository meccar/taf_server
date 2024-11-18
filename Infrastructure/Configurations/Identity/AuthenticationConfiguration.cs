using System.Security.Claims;
using System.Text;
using Infrastructure.Configurations.Environment;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Configurations.Identity;

public static class AuthenticationConfiguration
{
    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, EnvironmentConfiguration configuration)
    {
        services
            .AddAuthentication(options =>
                {
                    // options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
                    // options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
                    // options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            )
            .AddJwtBearer(options =>
            {
                options.Authority = configuration.GetIdentityServerAuthority();
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetJwtSecret())),
                    ValidateAudience = true,
                    ValidateIssuer = false,
                    ValidAudience = configuration.GetIdentityServerClientId(),
                    ValidIssuer = configuration.GetIdentityServerAuthority(),
                    ClockSkew = TimeSpan.Zero,

                };

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var identity = context.Principal.Identity as ClaimsIdentity;
                        if (identity == null) return;

                        // Add role claims if they don't exist
                        if (!identity.HasClaim(c => c.Type == ClaimTypes.Role))
                        {
                            var roleClaim = identity.Claims.FirstOrDefault(c => c.Type == "role");
                            if (roleClaim != null)
                            {
                                identity.AddClaim(new Claim(ClaimTypes.Role, roleClaim.Value));
                            }
                        }
                    }
                };
            });
        
        return services;
    }
}