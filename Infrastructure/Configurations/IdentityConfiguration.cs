using Microsoft.AspNetCore.Identity;
using taf_server.Domain.Aggregates;
using taf_server.Infrastructure.Data;

namespace taf_server.Infrastructure.Configurations;
public static class IdentityConfiguration
{
    public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
    {
        services
            .AddIdentity<UserAccountAggregate, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        //.AddTokenProvider<>()
        
        return services;
    }
}
