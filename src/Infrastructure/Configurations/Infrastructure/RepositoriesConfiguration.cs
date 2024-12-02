using Domain.Abstractions;
using Domain.Interfaces;
using Domain.Interfaces.Command;
using Domain.Interfaces.Query;
using Domain.Interfaces.Service;
using Domain.SeedWork.Command;
using Duende.IdentityServer.Services;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Command;
using Infrastructure.Repositories.Query;
using Infrastructure.Repositories.Service;
using Microsoft.Extensions.DependencyInjection;
using Share.Configurations.Environment;
using ITokenService = Domain.Interfaces.ITokenService;

namespace Infrastructure.Configurations.Infrastructure;

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
            .AddScoped<IJwtService, JwtService>()
            .AddScoped<IProfileService, ProfileService>()
            .AddScoped<IMfaService, MfaService>()
            .AddScoped<ITokenService, TokenService>();
        
        return services;
    }
}