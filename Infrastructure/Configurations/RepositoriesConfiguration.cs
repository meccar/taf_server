using FluentValidation;
using taf_server.Application.Usecases.Auth;
using taf_server.Domain.Abstractions;
using taf_server.Domain.Interfaces;
using taf_server.Domain.Interfaces.Command;
using taf_server.Domain.Interfaces.Query;
using taf_server.Domain.Repositories;
using taf_server.Infrastructure.Repositories;
using taf_server.Infrastructure.Repositories.Command;
using taf_server.Infrastructure.Repositories.Query;
using taf_server.Infrastructure.UseCaseProxy;
using taf_server.Presentations.Dtos.Authentication;
using taf_server.Presentations.Dtos.Authentication.Login;
using taf_server.Presentations.Dtos.Authentication.Register;
using taf_server.Presentations.Validators.Auth;

namespace taf_server.Infrastructure.Configurations;

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