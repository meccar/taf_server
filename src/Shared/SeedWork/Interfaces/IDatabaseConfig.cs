namespace Shared.SeedWork.Interfaces;

public interface IDatabaseConfig
{
    string GetDatabaseHost();
    int GetDatabasePort() ;
    string GetDatabasePassword() ;
    string GetDatabaseName() ;
    string GetDatabaseSchema() ;
    string GetDatabaseUserId();
    string GetMultipleActiveResultSets();
    bool GetTrustServerCertificate();
    bool GetTypeORMSync() ;
    bool GetTypeORMLogging() ;
    bool GetTypeORMMigrationsRun() ;
}