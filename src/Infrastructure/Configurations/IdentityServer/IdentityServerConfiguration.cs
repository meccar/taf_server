using System.Security.Cryptography.X509Certificates;
using Duende.IdentityServer.Configuration;
using Duende.IdentityServer.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Persistance.Repositories;
using Persistance.Repositories.User;
using Shared.Configurations.Environment;

namespace Infrastructure.Configurations.IdentityServer;

/// <summary>
/// Provides extension methods to configure Duende IdentityServer.
/// </summary>
public static class IdentityServerConfiguration
{
    /// <summary>
    /// Configures IdentityServer with specified options, including signing credentials, caching, and key management.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configuration">The environment configuration containing settings for IdentityServer.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> for chaining.</returns>
    /// <exception cref="Exception">Thrown if the signing certificate is not found.</exception>
    public static IServiceCollection ConfigureIdentityServer(this IServiceCollection services, EnvironmentConfiguration configuration)
    {
        var cert = new X509Certificate2("certificate.pfx", "tung");
        if (cert == null)
        {
            throw new Exception("Certificate not found.");
        }
        
        IdentityServerConfig.Initialize(configuration);

        services.AddIdentityServer(options =>
            {
                options.Caching.ClientStoreExpiration = TimeSpan.FromMinutes(5);
                options.Caching.ResourceStoreExpiration = TimeSpan.FromMinutes(5);
                
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                options.EmitStaticAudienceClaim = true;

                options.KeyManagement.Enabled = true;
                // new key every 30 days
                options.KeyManagement.RotationInterval = TimeSpan.FromDays(30);
                // announce new key 2 days in advance in discovery
                options.KeyManagement.PropagationTime = TimeSpan.FromDays(2);
                // keep old key for 7 days in discovery for validation of tokens
                options.KeyManagement.RetentionDuration = TimeSpan.FromDays(7);
                // don't delete keys after their retention period is over
                options.KeyManagement.DeleteRetiredKeys = false;
                // set path to store keys
                options.KeyManagement.KeyPath = Path.Combine(Directory.GetCurrentDirectory(), "keys");
                options.KeyManagement.SigningAlgorithms = new[]
                {
                    // RS256 for older clients (with additional X.509 wrapping)
                    new SigningAlgorithmOptions(SecurityAlgorithms.RsaSha256) { UseX509Certificate = true },
                    // PS256
                    new SigningAlgorithmOptions(SecurityAlgorithms.RsaSsaPssSha256),
                    // ES256
                    new SigningAlgorithmOptions(SecurityAlgorithms.EcdsaSha256)
                };
                options.ServerSideSessions.UserDisplayNameClaimType = "name";
                // options.LicenseKey = "eyJhbG...";
            })
            .AddSigningCredential(cert)
            .AddInMemoryClients(IdentityServerConfig.Clients ?? Array.Empty<Client>())
            .AddInMemoryIdentityResources(IdentityServerConfig.IdentityResources ?? Array.Empty<IdentityResource>())
            .AddInMemoryApiScopes(IdentityServerConfig.ApiScopes ?? Array.Empty<ApiScope>())
            .AddInMemoryApiResources(IdentityServerConfig.ApiResources ?? Array.Empty<ApiResource>())
            .AddTestUsers(IdentityServerConfig.TestUsers)
            .AddProfileService<ProfileService>()
            .AddServerSideSessions()
            .AddInMemoryCaching()
            // .AddAspNetIdentity<UserAccountAggregate>()
            .AddDeveloperSigningCredential();
            
        return services;
    }
}
