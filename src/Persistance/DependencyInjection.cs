// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
//
// namespace Persistance;
//
// public static class DependencyInjection
// {
//     public static IServiceCollection ConfigureDataBaseDependencyInjection(
//         this IServiceCollection services,
//         IConfiguration configuration
//         )
//     {
//         // var config = new EnvironmentConfiguration(configuration);
//
//         services.ConfigureDbContext(configuration);
//
//         return services;
//     }
// }