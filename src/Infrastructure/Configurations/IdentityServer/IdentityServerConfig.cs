using System.Security.Claims;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using IdentityModel;
using Infrastructure.Configurations.Environment;

namespace Infrastructure.Configurations.IdentityServer;

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
                new IdentityResources.Email(),
                // new IdentityResource
                // {
                //     Name = "roles",
                //     DisplayName = "Roles",
                //     UserClaims = new[] { ClaimTypes.Role }
                // }
            };

            // API scopes
            ApiScopes = new List<ApiScope>
            {
                new ApiScope("scopeV1"),
                new ApiScope("scopeV2"),
                new ApiScope("scopeV3"),
            };

            // Clients
            Clients = new List<Client>
            {
                // m2m client credentials flow client
                // new Client
                // {
                //     ClientId = configuration.GetIdentityServerClientId(),
                //     ClientName = configuration.GetIdentityServerClientName(),
                //     
                //     AllowedGrantTypes = GrantTypes.ClientCredentials,
                //     ClientSecrets =
                //     {
                //         new Secret(configuration.GetIdentityServerClientSecret().Sha256())
                //     },
                //     
                //     AllowedScopes =                 
                //     {
                //         IdentityServerConstants.StandardScopes.OpenId,
                //         IdentityServerConstants.StandardScopes.Email,
                //         IdentityServerConstants.StandardScopes.Profile
                //         // "openid",
                //         // "profile",
                //         // "api1", 
                //         // "read"
                //     },
                //     
                //     // where to redirect to after login
                //     // RedirectUris = { "https://localhost:6001/signin-oidc" },
                //
                //     // where to redirect to after logout
                //     // PostLogoutRedirectUris = { "https://localhost:6001/signout-callback-oidc" },
                //     
                //     AccessTokenLifetime = 3600, // 1 hour
                //     RefreshTokenUsage = TokenUsage.OneTimeOnly,
                //     RefreshTokenExpiration = TokenExpiration.Absolute,
                //     AbsoluteRefreshTokenLifetime = 86400, // 24 hours
                //
                //     AlwaysSendClientClaims = true,
                //     AlwaysIncludeUserClaimsInIdToken = true,
                //     UpdateAccessTokenClaimsOnRefresh = true
                // },
                
                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = configuration.GetIdentityServerInteractiveClientId(),
                    ClientName = configuration.GetIdentityServerInteractiveClientName(),
                    
                    AllowedGrantTypes = GrantTypes.Code,
                    
                    AllowRememberConsent = false,
                    RequireClientSecret = true,
                    ClientSecrets =
                    {
                        new Secret(configuration.GetIdentityServerInteractiveClientSecret().Sha256())
                    },
                    
                    RedirectUris = new List<string>
                    {
                        "https://localhost:6002/signin-oidc"
                    },
                    
                    FrontChannelLogoutUri = "https://localhost:6002/signout-oidc",
                    
                    PostLogoutRedirectUris = new List<string>
                    {
                        "https://localhost:6002/signout-callback-oidc"
                    },
                    
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "scopeV1",
                        "scopeV2",
                        "scopeV3"
                    },
                    AllowOfflineAccess = true
                }
            };
            
            ApiResources = new List<ApiResource>
            {
                new ApiResource("tafApiV1")
                {
                    Scopes = { "scopeV1" },
                    UserClaims = new[]
                    {
                        ClaimTypes.Email,
                    },
                    ApiSecrets =
                    {
                        new Secret(configuration.GetIdentityServerInteractiveClientSecret().Sha256())
                    }
                },
                new ApiResource("tafApiV2")
                {
                    Scopes = { "scopeV2" },
                    UserClaims = new[]
                    {
                        ClaimTypes.Email,
                    },
                    ApiSecrets =
                    {
                        new Secret(configuration.GetIdentityServerInteractiveClientSecret().Sha256())
                    }
                },
                new ApiResource("tafApiV3")
                {
                    Scopes = { "scopeV3" },
                    UserClaims = new[]
                    {
                        ClaimTypes.Email,
                    },
                    ApiSecrets =
                    {
                        new Secret(configuration.GetIdentityServerInteractiveClientSecret().Sha256())
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