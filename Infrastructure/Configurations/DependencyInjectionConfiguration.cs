using FluentValidation;
using taf_server.Domain.Abstractions;
using taf_server.Domain.Interfaces;
using taf_server.Domain.Repositories;
using taf_server.Infrastructure.Repositories;
using taf_server.Infrastructure.Repositories.Command;
using taf_server.Presentations.Dtos.Authentication;
using taf_server.Presentations.Validators.Auth;

namespace taf_server.Infrastructure.Configurations;

public static class DependencyInjectionConfiguration
{
    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services)
    {
        services
            .AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>))
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<IValidator<RegisterUserRequestDto>, RegisterValidator>()
            .AddScoped<IUserAccountCommandRepository, UserAccountCommandRepository>()
            .AddScoped<IUserLoginDataCommandRepository, UserLoginDataCommandRepository>();
        
        return services;
    }
}