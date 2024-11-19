using System.Security.Claims;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Infrastructure.Configurations.Environment;

namespace Infrastructure.Configurations.Identity;

public static class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> IdentityResources { get; set; }
        public static IEnumerable<ApiScope> ApiScopes { get; set; }
        public static IEnumerable<Client> Clients { get; set; }
        public static IEnumerable<ApiResource> ApiResources { get; set; }

        public static void Initialize(EnvironmentConfiguration configuration)
        {
            // Identity resources
            IdentityResources = new List<IdentityResource>
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
            ApiScopes = new List<ApiScope>
            {
                new ApiScope("api1", "My API"),
                new ApiScope(name: "read",   displayName: "Read your data."),
                new ApiScope(name: "write",  displayName: "Write your data."),
                new ApiScope(name: "delete", displayName: "Delete your data.")
            };

            // Clients
            Clients = new List<Client>
            {
                new Client
                {
                    ClientId = configuration.GetIdentityServerClientId(),
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes =                 
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Profile
                        // "openid",
                        // "profile",
                        // "api1", 
                        // "read"
                    },
                    
                    // where to redirect to after login
                    // RedirectUris = { "https://localhost:5002/signin-oidc" },

                    // where to redirect to after logout
                    // PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },
                    
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
            
            ApiResources = new List<ApiResource>
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