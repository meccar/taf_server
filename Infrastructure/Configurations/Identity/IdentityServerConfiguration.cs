using System.Security.Cryptography.X509Certificates;
using Duende.IdentityServer.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Configurations.Environment;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Configurations.Identity;
public static class IdentityServerConfiguration
{
    public static IServiceCollection ConfigureIdentityServer(this IServiceCollection services, EnvironmentConfiguration configuration)
    {
        var cert = new X509Certificate2("../certificate.pfx", "tung");
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
            .AddInMemoryClients(IdentityServerConfig.Clients)
            .AddInMemoryIdentityResources(IdentityServerConfig.IdentityResources)
            .AddInMemoryApiScopes(IdentityServerConfig.ApiScopes)
            .AddInMemoryApiResources(IdentityServerConfig.ApiResources)
            .AddTestUsers(IdentityServerConfig.TestUsers)
            .AddServerSideSessions()
            .AddInMemoryCaching()
            // .AddAspNetIdentity<UserLoginDataEntity>()
            .AddDeveloperSigningCredential();
            
        return services;
    }
}
