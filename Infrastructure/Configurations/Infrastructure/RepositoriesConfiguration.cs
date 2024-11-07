using Domain.Abstractions;
using Domain.Interfaces;
using Domain.Interfaces.Command;
using Domain.Interfaces.Query;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Command;
using Infrastructure.Repositories.Query;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configurations.Infrastructure;

public static class RepositoriesConfiguration
{
    public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
    {
        services
            .AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>))
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<IUserAccountCommandRepository, UserAccountCommandRepository>()
            .AddScoped<IUserLoginDataCommandRepository, UserLoginDataCommandRepository>()
            .AddScoped<IUserAccountQueryRepository, UserAccountQueryRepository>()
            .AddScoped<IUserLoginDataQueryRepository, UserLoginDataQueryRepository>();
        
        return services;
    }
}