using System.Text;
using Duende.IdentityServer;
using IdentityModel;
using Infrastructure.Configurations.Environment;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

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
                    // options.DefaultAuthenticateScheme = OpenIdConnectDefaults.AuthenticationScheme;
                    // options.DefaultSignInScheme = OpenIdConnectDefaults.AuthenticationScheme;
                    options.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
                }
            )
            // .AddGoogle(options =>
            // {
            // options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

            // options.ClientId = configuration.GetGoogleClientId();
            // options.ClientSecret = configuration.GetGoogleClientSecret();

            // })
            .AddJwtBearer(configuration.GetJwtType(), options =>
            {
                options.Authority = configuration.GetIdentityServerAuthority();
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetJwtSecret())),
                    ValidateAudience = true,
                    ValidateIssuer = false,
                    ValidIssuer = configuration.GetIdentityServerAuthority(),
                    ValidAudience = configuration.GetIdentityServerInteractiveClientId(),
                    ClockSkew = TimeSpan.Zero,
                };
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {

                // host prefixed cookie name
                options.Cookie.Name = "__Host-Standalone-bff";

                // strict SameSite handling
                options.Cookie.SameSite = SameSiteMode.Strict;

                // set session lifetime
                options.ExpireTimeSpan = TimeSpan.FromHours(8);

                // sliding or absolute
                options.SlidingExpiration = false;
            })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Authority = configuration.GetIdentityServerAuthority();
                
                options.ClientId = configuration.GetIdentityServerInteractiveClientId();
                options.ClientSecret = configuration.GetIdentityServerInteractiveClientSecret();
                
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.ResponseMode = OpenIdConnectResponseMode.Query;
                
                // var scopes = configuration.GetIdentityServerScopes().Split(' ');
                // foreach (var scope in scopes)
                // {
                //     options.Scope.Add(scope);
                // }
                
                options.Scope.Clear();
                options.Scope.Add(IdentityServerConstants.StandardScopes.OpenId);
                options.Scope.Add(IdentityServerConstants.StandardScopes.Profile);
                options.Scope.Add(IdentityServerConstants.StandardScopes.Email);
                
                // refresh token
                options.Scope.Add(IdentityServerConstants.StandardScopes.OfflineAccess);
            
                options.CallbackPath = "/signin-oidc";
                options.SignedOutCallbackPath = "/signout-callback-oidc";

                
                options.UseTokenLifetime = true;
                options.MapInboundClaims = false;
            
                options.GetClaimsFromUserInfoEndpoint = true;
                options.SaveTokens = true;
                
                options.Events = new OpenIdConnectEvents
                {
                    OnRemoteFailure = (context) =>
                    {
                        // Log detailed error information
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = (context) =>
                    {
                        // Log authentication failure details
                        Console.WriteLine($"OpenID Connect Authentication Failed: {context.Exception?.Message}");
                        Console.WriteLine($"Exception Details: {context.Exception?.ToString()}");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = (context) =>
                    {
                        // Log successful token validation
                        Console.WriteLine("Token successfully validated");
                        return Task.CompletedTask;
                    }
                };

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.GivenName
                };
            });
        
        return services;
    }
}