using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Domain.Entities;
using Duende.IdentityServer.Configuration;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Duende.IdentityServer.Models;
using Microsoft.IdentityModel.Tokens;


namespace Infrastructure.Configurations.Identity;
public static class IdentityServerConfiguration
{
    public static IServiceCollection ConfigureIdentityServer(this IServiceCollection services)
    {
        var cert = new X509Certificate2("../certificate.pfx", "tung");
        if (cert == null)
        {
            throw new Exception("Certificate not found.");
        }
        
        services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                
                options.EmitStaticAudienceClaim = true;
                
                // new key every 30 days
                options.KeyManagement.RotationInterval = TimeSpan.FromDays(30);
                // announce new key 2 days in advance in discovery
                options.KeyManagement.PropagationTime = TimeSpan.FromDays(2);
                // keep old key for 7 days in discovery for validation of tokens
                options.KeyManagement.RetentionDuration = TimeSpan.FromDays(7);
                // don't delete keys after their retention period is over
                options.KeyManagement.DeleteRetiredKeys = false;
                // set path to store keys
                options.KeyManagement.KeyPath = "/home/shared/keys";
                options.KeyManagement.SigningAlgorithms = new[]
                {
                    // RS256 for older clients (with additional X.509 wrapping)
                    new SigningAlgorithmOptions(SecurityAlgorithms.RsaSha256) { UseX509Certificate = true },
                    // PS256
                    new SigningAlgorithmOptions(SecurityAlgorithms.RsaSsaPssSha256),
                    // ES256
                    new SigningAlgorithmOptions(SecurityAlgorithms.EcdsaSha256)
                };
                // options.LicenseKey = "eyJhbG...";
            })
            .AddSigningCredential(cert)
            .AddInMemoryClients(IdentityServerConfig.Clients)
            .AddInMemoryIdentityResources(IdentityServerConfig.IdentityResources)
            .AddInMemoryApiScopes(IdentityServerConfig.ApiScopes)
            .AddInMemoryApiResources(IdentityServerConfig.ApiResources)
            .AddDeveloperSigningCredential();

        return services;
    }

    private static class IdentityServerConfig
    {
        // Identity resources
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = "roles",
                    DisplayName = "Roles",
                    UserClaims = new[] { ClaimTypes.Role }
                }
            };

        // API scopes
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("api1", "My API"),
                new ApiScope(name: "read",   displayName: "Read your data."),
                new ApiScope(name: "write",  displayName: "Write your data."),
                new ApiScope(name: "delete", displayName: "Delete your data.")
            };

        // Clients
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes =                 
                    {
                        "openid",
                        "profile",
                        "api1", 
                        "read"
                    },
                    
                    AccessTokenLifetime = 3600, // 1 hour
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = 86400, // 24 hours
                
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    UpdateAccessTokenClaimsOnRefresh = true
                },
                new Client
                {
                    ClientId = "interactive_client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    RequirePkce = true,
                    RequireClientSecret = true,
                    ClientSecrets =
                    {
                        new Secret("interactive_secret".Sha256())
                    },
                    RedirectUris = { "https://localhost:5001/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },
                    AllowedScopes = { "openid", "profile", "api1" },
                    AllowOfflineAccess = true
                }
            };
        
        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("api1", "My API")
                {
                    Scopes = { "api1" },
                    UserClaims = new[]
                    {
                        ClaimTypes.Role,
                        ClaimTypes.Email,
                        "eid"
                    }
                }
            };
    }
}
