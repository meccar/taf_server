// using DataBase.Configurations;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Share.Configurations.Environment;
//
// namespace DataBase;
//
// public static class DependencyInjection
// {
//     public static IServiceCollection ConfigureDataBaseDependencyInjection(
//         this IServiceCollection services,
//         IConfiguration configuration,
//         string appCors)
//     {
//         var config = new EnvironmentConfiguration(configuration);
//
//         services.ConfigureDbContext(config);
//
//         return services;
//     }
// }