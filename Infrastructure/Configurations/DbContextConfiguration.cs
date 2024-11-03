using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configurations;

public static class DbContextConfiguration
{
    public static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
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
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException("DefaultConnection is not configured.");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(
                connectionString,
                //"Server=localhost,14407;Database=master;User Id=sa;Password=Admin@123;MultipleActiveResultSets=True;TrustServerCertificate=True;",
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 10,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                    sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                });
        });
        
        return services;
    }
}