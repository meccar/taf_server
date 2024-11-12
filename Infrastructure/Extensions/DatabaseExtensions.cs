using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Infrastructure.Extensions;

public static class DatabaseExtensions
{
    public static async Task UseReplicationAsync(this DatabaseFacade database)
    {
        var connectionString = database.GetDbConnection().ConnectionString;

        var replicaConnectionString = GetReplicaConnectionString(connectionString);

        var optionsBuilder = new DbContextOptionsBuilder<DbContext>()
            .UseSqlServer(replicaConnectionString);
        
        var context = new DbContext(optionsBuilder.Options);

        await Task.CompletedTask; 
    }

    private static string GetReplicaConnectionString(string primaryConnectionString)
    {
        return primaryConnectionString.Replace("PrimaryServer", "ReplicaServer");
    }
}