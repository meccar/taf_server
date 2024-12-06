namespace Domain.Interfaces;

/// <summary>
/// Defines the contract for database configuration settings.
/// This interface is used to retrieve various database-related configuration values.
/// </summary>
public interface IDatabaseConfig
{
    /// <summary>
    /// Gets the host address of the database server.
    /// </summary>
    /// <returns>The database host address.</returns>
    string GetDatabaseHost();

    /// <summary>
    /// Gets the port number used for database communication.
    /// </summary>
    /// <returns>The database port number.</returns>
    int GetDatabasePort();

    /// <summary>
    /// Gets the password for accessing the database.
    /// </summary>
    /// <returns>The database password.</returns>
    string GetDatabasePassword();

    /// <summary>
    /// Gets the name of the database.
    /// </summary>
    /// <returns>The name of the database.</returns>
    string GetDatabaseName();

    /// <summary>
    /// Gets the schema name used in the database.
    /// </summary>
    /// <returns>The database schema name.</returns>
    string GetDatabaseSchema();

    /// <summary>
    /// Gets the user ID used for database authentication.
    /// </summary>
    /// <returns>The database user ID.</returns>
    string GetDatabaseUserId();

    /// <summary>
    /// Gets whether multiple active result sets (MARS) are enabled for the database connection.
    /// </summary>
    /// <returns>A string indicating if MARS is enabled.</returns>
    string GetMultipleActiveResultSets();

    /// <summary>
    /// Gets whether the database connection should trust the server certificate.
    /// </summary>
    /// <returns>A boolean indicating if the server certificate is trusted.</returns>
    bool GetTrustServerCertificate();

    /// <summary>
    /// Gets whether synchronization for TypeORM is enabled.
    /// </summary>
    /// <returns>A boolean indicating if TypeORM synchronization is enabled.</returns>
    bool GetTypeORMSync();

    /// <summary>
    /// Gets whether logging is enabled for TypeORM.
    /// </summary>
    /// <returns>A boolean indicating if TypeORM logging is enabled.</returns>
    bool GetTypeORMLogging();

    /// <summary>
    /// Gets whether migrations are automatically run for TypeORM.
    /// </summary>
    /// <returns>A boolean indicating if TypeORM migrations are enabled to run automatically.</returns>
    bool GetTypeORMMigrationsRun();
}