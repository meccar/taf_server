namespace Shared.SeedWork.Interfaces;

/// <summary>
/// Represents the configuration settings for a database connection.
/// </summary>
public interface IDatabaseConfig
{
    /// <summary>
    /// Gets the database host address.
    /// </summary>
    /// <returns>The database host as a string, or null if not configured.</returns>
    string? GetDatabaseHost();

    /// <summary>
    /// Gets the port number used for the database connection.
    /// </summary>
    /// <returns>The port number as an integer.</returns>
    int GetDatabasePort();

    /// <summary>
    /// Gets the password for the database user.
    /// </summary>
    /// <returns>The database password as a string, or null if not configured.</returns>
    string? GetDatabasePassword();

    /// <summary>
    /// Gets the name of the database to connect to.
    /// </summary>
    /// <returns>The database name as a string, or null if not configured.</returns>
    string? GetDatabaseName();

    /// <summary>
    /// Gets the schema used by the database.
    /// </summary>
    /// <returns>The database schema as a string, or null if not configured.</returns>
    string? GetDatabaseSchema();

    /// <summary>
    /// Gets the user ID for the database connection.
    /// </summary>
    /// <returns>The database user ID as a string, or null if not configured.</returns>
    string? GetDatabaseUserId();

    /// <summary>
    /// Gets the configuration for multiple active result sets (MARS).
    /// </summary>
    /// <returns>A string indicating whether MARS is enabled, or null if not configured.</returns>
    string? GetMultipleActiveResultSets();

    /// <summary>
    /// Determines whether the server certificate should be trusted without validation.
    /// </summary>
    /// <returns>True if the server certificate should be trusted; otherwise, false.</returns>
    bool GetTrustServerCertificate();

    /// <summary>
    /// Determines whether TypeORM synchronization is enabled.
    /// </summary>
    /// <returns>True if TypeORM synchronization is enabled; otherwise, false.</returns>
    bool GetTypeOrmSync();

    /// <summary>
    /// Determines whether TypeORM logging is enabled.
    /// </summary>
    /// <returns>True if TypeORM logging is enabled; otherwise, false.</returns>
    bool GetTypeOrmLogging();

    /// <summary>
    /// Determines whether TypeORM migrations should run automatically.
    /// </summary>
    /// <returns>True if migrations should run automatically; otherwise, false.</returns>
    bool GetTypeOrmMigrationsRun();
}
