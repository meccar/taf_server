using Application.Commands.Auth.Register;
using Application.Usecases.Auth;
using Domain.SeedWork.Command;
using Infrastructure.UseCaseProxy;
using Microsoft.Extensions.DependencyInjection;
using Shared.Dtos.Authentication.Login;
using Shared.Dtos.Authentication.Register;
using Shared.Model;

namespace Application.Configurations;

public static class UsecasesConfiguration
{
    public static IServiceCollection ConfigureUsecases(this IServiceCollection services)
    {
        services
            .AddScoped<RegisterUsecase>()
            .AddScoped<LoginUsecase>()
            .AddScoped<ICommandHandler<RegisterCommand, UserAccountModel>, RegisterCommandHandler>()
            // .Decorate<ICommandHandler<RegisterCommand, UserAccountModel>, 
            //     TransactionalDecorator<RegisterCommand, UserAccountModel>>()
            .AddScoped<UseCaseProxy<RegisterUsecase, RegisterUserRequestDto, RegisterUserResponseDto>>(provider =>
                new UseCaseProxy<RegisterUsecase, RegisterUserRequestDto, RegisterUserResponseDto>(
                    provider.GetRequiredService<RegisterUsecase>()))
            .AddScoped<UseCaseProxy<LoginUsecase, LoginUserRequestDto, LoginResponseDto>>(provider =>
                new UseCaseProxy<LoginUsecase, LoginUserRequestDto, LoginResponseDto>(
                    provider.GetRequiredService<LoginUsecase>()));
        
        return services;
    }
}