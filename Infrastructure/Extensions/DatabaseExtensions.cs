using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class DatabaseExtensions
{
    public static async Task UseReplicationAsync(this DatabaseFacade database)
    {
        // For example, assume you're using SQL Server's Always On Availability Groups
        // You would set the database connection to a read replica

        var connectionString = database.GetDbConnection().ConnectionString;

        // In a typical case, the connection string for the read replica would be different
        var replicaConnectionString = GetReplicaConnectionString(connectionString);

        // You might need to create a new DbContext or adjust the current DbContext to use the replica
        var optionsBuilder = new DbContextOptionsBuilder<DbContext>()
            .UseSqlServer(replicaConnectionString);  // Or UseNpgsql() for PostgreSQL
        
        var context = new DbContext(optionsBuilder.Options);

        await Task.CompletedTask; 
    }

    private static string GetReplicaConnectionString(string primaryConnectionString)
    {
        // Replace primary connection with the replica connection string
        return primaryConnectionString.Replace("PrimaryServer", "ReplicaServer");
    }
}