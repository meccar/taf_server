using Infrastructure.Configurations.Environment;
using Infrastructure.Data;
using Infrastructure.Decorators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configurations.Database;

public static class DbContextConfiguration
{
    public static IServiceCollection ConfigureDbContext(this IServiceCollection services, EnvironmentConfiguration configuration)
        // {
        //     // services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        //     // services.AddSingleton<DateTrackingInterceptor>();
        //
        //     var connectionString = configuration.GetConnectionString("DefaultConnectionString");
        //     if (connectionString == null || string.IsNullOrEmpty(connectionString))
        //         throw new ArgumentNullException("DefaultConnectionString is not configured.");
        //     services.AddDbContext<ApplicationDbContext>((provider, optionsBuilder) =>
        //     {
        //         var convertDomainEventsToOutboxMessagesInterceptor =
        //             provider.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>()!;
        //         var dateTrackingInterceptor =
        //             provider.GetService<DateTrackingInterceptor>()!;
        //
        //         optionsBuilder
        //             .UseSqlServer(connectionString, builder => 
        //                 builder.MigrationsAssembly("EventHub.Persistence"))
        //             .AddInterceptors(
        //                 dateTrackingInterceptor,
        //                 convertDomainEventsToOutboxMessagesInterceptor
        //             );
        //     });
        //     return services;
        // }
    {
        var connectionString = 
            $"Server={configuration.GetDatabaseHost()}," +
            $"{configuration.GetDatabasePort()};" +
            $"Database={configuration.GetDatabaseName()};" +
            $"User Id={configuration.GetDatabaseUserId()};" +
            $"Password={configuration.GetDatabasePassword()};" +
            $"MultipleActiveResultSets={configuration.GetMultipleActiveResultSets()};" +
            $"TrustServerCertificate={configuration.GetTrustServerCertificate()};";

        services.AddDbContextPool<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(
                connectionString,
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 10,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                    sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                });
        });
        
        services.AddPooledDbContextFactory<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(
                connectionString,
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 10,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                    sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                });
        });
        
        services.AddScoped<TransactionDecorator>();
        
        return services;
    }
}