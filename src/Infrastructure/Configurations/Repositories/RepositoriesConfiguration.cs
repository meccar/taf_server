using Domain.Abstractions;
using Domain.Interfaces;
using Domain.Interfaces.Service;
using Duende.IdentityServer.Services;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Service;
using Microsoft.Extensions.DependencyInjection;
using Shared.Configurations.Environment;

namespace Infrastructure.Configurations.Repositories;

public static class RepositoriesConfiguration
{
    public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
    {
        services
            .AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>))
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<EnvironmentConfiguration>()
            .AddScoped<IUserAccountRepository, UserAccountRepository>()
            .AddScoped<IUserProfileRepository, UserProfileRepository>()
            .AddScoped<IUserTokenRepository, UserTokenRepository>()
            .AddScoped<IJwtRepository, JwtRepository>()
            .AddScoped<IProfileService, ProfileService>()
            .AddScoped<IMfaRepository, MfaRepository>()
            .AddScoped<ITokenRepository, TokenRepository>();
        
        return services;
    }
}