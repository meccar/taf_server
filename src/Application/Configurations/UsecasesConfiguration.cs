using Application.Dtos.Authentication.Login;
using Application.Dtos.Authentication.Register;
using Application.Usecases.Auth;
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
            .AddScoped<UseCaseProxy<RegisterUsecase, RegisterUserRequestDto, RegisterUserResponseDto>>(provider =>
                new UseCaseProxy<RegisterUsecase, RegisterUserRequestDto, RegisterUserResponseDto>(
                    provider.GetRequiredService<RegisterUsecase>()))
            .AddScoped<UseCaseProxy<LoginUsecase, LoginUserRequestDto, LoginResponseDto>>(provider =>
                new UseCaseProxy<LoginUsecase, LoginUserRequestDto, LoginResponseDto>(
                    provider.GetRequiredService<LoginUsecase>()));
        
        return services;
    }
}