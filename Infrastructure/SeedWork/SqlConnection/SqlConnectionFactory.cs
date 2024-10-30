// using System.Data;
// using Infrastructure.Abstractions;
// using Microsoft.Extensions.Configuration;
//
// namespace Infrastructure.SeedWork.SqlConnection;
//
// public class SqlConnectionFactory : ISqlConnectionFactory
// {
//     private readonly string _connectionString;
//
//     public SqlConnectionFactory(IConfiguration configuration)
//     {
//         _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
//     }
//
//     public IDbConnection CreateConnection()
//     {
//         var connection = new MySqlConnection(_connectionString);
//         connection.Open();
//         return connection;
//     }
// }