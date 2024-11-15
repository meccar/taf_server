using Domain.Abstractions;
using Domain.Interfaces;
using Domain.Interfaces.Command;
using Domain.Interfaces.Query;
using Domain.Interfaces.Service;
using Infrastructure.Configurations.Environment;
using Infrastructure.Extensions;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Command;
using Infrastructure.Repositories.Query;
using Infrastructure.Repositories.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configurations.Infrastructure;

public static class RepositoriesConfiguration
{
    public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
    {
        services
            .AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>))
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<EnvironmentConfiguration>()
            .AddScoped<IUserAccountCommandRepository, UserAccountCommandRepository>()
            .AddScoped<IUserLoginDataCommandRepository, UserLoginDataCommandRepository>()
            .AddScoped<IUserTokenCommandRepository, UserTokenCommandRepository>()
            .AddScoped<IUserAccountQueryRepository, UserAccountQueryRepository>()
            .AddScoped<IUserTokenQueryRepository, UserTokenQueryRepository>()
            .AddScoped<IUserLoginDataQueryRepository, UserLoginDataQueryRepository>()
            .AddScoped<IJwtService, JwtService>()
            .AddScoped<ITokenService, TokenService>();
        
        return services;
    }
}