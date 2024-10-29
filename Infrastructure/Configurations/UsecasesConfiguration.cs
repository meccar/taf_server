using taf_server.Application.Usecases.Auth;
using taf_server.Infrastructure.UseCaseProxy;
using taf_server.Presentations.Dtos.Authentication.Login;
using taf_server.Presentations.Dtos.Authentication.Register;

namespace taf_server.Infrastructure.Configurations;

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