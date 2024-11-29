using Application.Commands.Auth.Register;
using Application.Dtos.Authentication.Login;
using Application.Dtos.Authentication.Register;
using Application.Usecases.Auth;
using Domain.Model;
using Domain.SeedWork.Command;
using Infrastructure.UseCaseProxy;
using Microsoft.Extensions.DependencyInjection;

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