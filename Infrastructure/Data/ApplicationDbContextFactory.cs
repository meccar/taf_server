using Infrastructure.Configurations.Environment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", optional: true)
            .Build();
        
        var environmentConfiguration = new EnvironmentConfiguration(configuration);
        
        var connectionString = 
            $"Server={environmentConfiguration.GetDatabaseHost()};" +
            $"Database={environmentConfiguration.GetDatabaseName()};" +
            $"User Id={environmentConfiguration.GetDatabaseUserId()};" +
            $"Password={environmentConfiguration.GetDatabasePassword()};" +
            $"MultipleActiveResultSets={environmentConfiguration.GetMultipleActiveResultSets()};" +
            $"TrustServerCertificate={environmentConfiguration.GetTrustServerCertificate()};";
        
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
            sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
        });

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}