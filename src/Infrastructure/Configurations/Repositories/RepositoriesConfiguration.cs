// using Domain.Abstractions;
// using Domain.Interfaces;
// using Duende.IdentityServer.Services;
// using Microsoft.Extensions.DependencyInjection;
// using Persistance.Repositories;
// using Shared.Configurations.Environment;
//
// namespace Infrastructure.Configurations.Repositories;
//
// /// <summary>
// /// Provides extension methods to configure repositories in the application.
// /// </summary>
// public static class RepositoriesConfiguration
// {
//     /// <summary>
//     /// Configures the repositories and their dependencies in the application.
//     /// </summary>
//     /// <param name="services">The <see cref="IServiceCollection"/> to add repository services to.</param>
//     /// <returns>The modified <see cref="IServiceCollection"/> for chaining.</returns>
//     public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
//     {
//         services
//             .AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>))
//             .AddScoped<IUnitOfWork, UnitOfWork>()
//             .AddScoped<EnvironmentConfiguration>()
//             .AddScoped<IUserAccountRepository, UserAccountRepository>()
//             .AddScoped<IUserProfileRepository, UserProfileRepository>()
//             .AddScoped<IUserTokenRepository, UserTokenRepository>()
//             .AddScoped<IJwtRepository, JwtRepository>()
//             .AddScoped<IProfileService, ProfileService>()
//             .AddScoped<IMfaRepository, MfaRepository>()
//             .AddScoped<IMailRepository, MailRepository>()
//             .AddScoped<ITokenRepository, TokenRepository>();
//         
//         return services;
//     }
// }