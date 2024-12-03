using Application.Commands.Auth.Register;
using Application.Usecases.Auth;
using Domain.SeedWork.Command;
using Microsoft.Extensions.DependencyInjection;
using Shared.Model;

namespace Application.Configurations;

public static class UsecasesConfiguration
{
    public static IServiceCollection ConfigureUsecases(this IServiceCollection services)
    {
        services
            .AddScoped<RegisterUsecase>()
            .AddScoped<LoginUsecase>()
            .AddScoped<ICommandHandler<RegisterCommand, UserProfileModel>, RegisterCommandHandler>()
            .AddScoped<RegisterUsecase>()
            .AddScoped<LoginUsecase>();
        
        return services;
    }
}