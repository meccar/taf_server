using Domain.Abstractions;
using Domain.Interfaces;
using Domain.Interfaces.Command;
using Domain.Interfaces.Query;
using Infrastructure.Configurations.Environment;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Command;
using Infrastructure.Repositories.Query;
using Infrastructure.Repositories.Service;
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
            .AddTransient<IJwtService, JwtService>()
            .AddScoped<IUserAccountCommandRepository, UserAccountCommandRepository>()
            .AddScoped<IUserLoginDataCommandRepository, UserLoginDataCommandRepository>()
            .AddScoped<IUserAccountQueryRepository, UserAccountQueryRepository>()
            .AddScoped<IUserTokenCommandRepository, UserTokenCommandRepository>()
            .AddScoped<IUserLoginDataQueryRepository, UserLoginDataQueryRepository>();
        
        return services;
    }
}