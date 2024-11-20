using System.Security.Claims;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using IdentityModel;
using Infrastructure.Configurations.Environment;

namespace Infrastructure.Configurations.Identity;

public static class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> IdentityResources { get; set; }
        public static IEnumerable<ApiScope> ApiScopes { get; set; }
        public static IEnumerable<Client> Clients { get; set; }
        public static IEnumerable<ApiResource> ApiResources { get; set; }
        public static List<TestUser> TestUsers { get; set; }

        public static void Initialize(EnvironmentConfiguration configuration)
        {
            // Identity resources
            IdentityResources = new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                // new IdentityResources.Email(),
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
                    ClientName = configuration.GetIdentityServerClientName(),
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret(configuration.GetIdentityServerClientSecret().Sha256())
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
                    // RedirectUris = { "https://localhost:6001/signin-oidc" },

                    // where to redirect to after logout
                    // PostLogoutRedirectUris = { "https://localhost:6001/signout-callback-oidc" },
                    
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
                    ClientId = configuration.GetIdentityServerMvcClientId(),
                    ClientName = configuration.GetIdentityServerMvcClientName(),
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowRememberConsent = false,
                    RequireClientSecret = true,
                    ClientSecrets =
                    {
                        new Secret(configuration.GetIdentityServerMvcClientSecret().Sha256())
                    },
                    RedirectUris = new List<string>
                    {
                        $"{configuration.GetIdentityServerAuthority()}"+"/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        $"{configuration.GetIdentityServerAuthority()}"+"/signout-callback-oidc"
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
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

            TestUsers = new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "11111",
                    Username = "jane2@example.com",
                    Password = "HGOSFUgiodfby8^&*&",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.GivenName, "Test User"),
                        new Claim(JwtClaimTypes.FamilyName, "admin")
                    }
                }

            };
        }
    }