// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Infrastructure;
//
// namespace Infrastructure.Extensions;
//
// /// <summary>
// /// Provides extension methods for interacting with the database.
// /// </summary>
// public static class DatabaseExtensions
// {
//     /// <summary>
//     /// Configures the database context to use a replica server for replication.
//     /// </summary>
//     /// <param name="database">The <see cref="DatabaseFacade"/> to configure for replication.</param>
//     /// <returns>A task representing the asynchronous operation.</returns>
//     public static async Task UseReplicationAsync(this DatabaseFacade database)
//     {
//         var connectionString = database.GetDbConnection().ConnectionString;
//
//         var replicaConnectionString = GetReplicaConnectionString(connectionString);
//
//         var optionsBuilder = new DbContextOptionsBuilder<DbContext>()
//             .UseSqlServer(replicaConnectionString);
//         
//         var context = new DbContext(optionsBuilder.Options);
//
//         await Task.CompletedTask; 
//     }
//
//     /// <summary>
//     /// Generates a replica connection string based on the primary connection string.
//     /// </summary>
//     /// <param name="primaryConnectionString">The primary connection string to replicate.</param>
//     /// <returns>The modified replica connection string.</returns>
//     private static string GetReplicaConnectionString(string primaryConnectionString)
//     {
//         return primaryConnectionString.Replace("PrimaryServer", "ReplicaServer");
//     }
// }