using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Data;
using Shared.Configurations.Environment;

namespace Infrastructure.Configurations.DataBase;

/// <summary>
/// Provides methods for configuring database context and related services.
/// </summary>
public static class DbContextConfiguration
{
    /// <summary>
    /// Configures the application's database context using the specified environment configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the database configuration to.</param>
    /// <param name="configuration">The environment configuration containing database settings.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> with the database context configured.</returns>
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

        services
            .AddScoped<ApplicationDbContextSeed>();
        
        // services.AddScoped<IDbContextTransaction>(sp =>
        //     new SqlConnection(connectionString));
        
        // services.AddConfigurationStore(options => options.ConnectionString = connectionString);
        
        // services.AddScoped<TransactionDecorator>();
        
        return services;
    }
}