using System.Security.Claims;
using System.Text;
using Duende.IdentityServer;
using Infrastructure.Configurations.Environment;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Google;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
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
                    // options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            )
            // .AddGoogle(options =>
            // {
                // options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                
                // options.ClientId = configuration.GetGoogleClientId();
                // options.ClientSecret = configuration.GetGoogleClientSecret();

            // })
            .AddJwtBearer(configuration.GetJwtType(),options =>
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
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Authority = configuration.GetIdentityServerAuthority();
                
                options.ClientId = configuration.GetIdentityServerMvcClientId();
                options.ClientSecret = configuration.GetIdentityServerMvcClientSecret();
                options.ResponseType = "code";
                // options.ResponseMode = "form_post";
                
                // var scopes = configuration.GetIdentityServerScopes().Split(' ');
                // options.Scope.Clear();
                // foreach (var scope in scopes)
                // {
                //     options.Scope.Add(scope);
                // }
                options.Scope.Add(IdentityServerConstants.StandardScopes.OpenId);
                options.Scope.Add(IdentityServerConstants.StandardScopes.Profile);
                
                options.UseTokenLifetime = true;
                options.MapInboundClaims = false;

                options.GetClaimsFromUserInfoEndpoint = true;
                options.SaveTokens = true;
            });
        
        return services;
    }
}