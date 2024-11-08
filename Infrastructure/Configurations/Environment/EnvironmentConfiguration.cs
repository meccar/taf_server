using Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Configurations.Environment;

public class EnvironmentConfiguration : IDatabaseConfig, IJWTConfig
{
    private readonly IConfiguration _configuration;
    public EnvironmentConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    #region Database
    public string GetDatabaseHost()
    {
        return _configuration.GetValue<string>("ConnectionStrings:Host");
    }
    public int GetDatabasePort()
    {
        return _configuration.GetValue<int>("ConnectionStrings:Port");
    }
    public string GetDatabaseName()
    {
        return _configuration.GetValue<string>("ConnectionStrings:Database");
    }
    public string GetDatabaseUserId()
    {
        return _configuration.GetValue<string>("ConnectionStrings:UserId");
    }
    public string GetDatabasePassword()
    {
        return _configuration.GetValue<string>("ConnectionStrings:Password");
    }
    public string GetMultipleActiveResultSets()
    {
        return _configuration.GetValue<string>("ConnectionStrings:MultipleActiveResultSets");
    }
    public bool GetTrustServerCertificate()
    {
        return _configuration.GetValue<bool>("ConnectionStrings:TrustServerCertificate");
    }
    public string GetDatabaseSchema()
    {
        return _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
    }
    #endregion
    
    #region Type ORM
    public bool GetTypeORMSync()
    {
        return _configuration.GetValue<bool>("TypeORMSettings:Sync");
    }

    public bool GetTypeORMLogging()
    {
        return _configuration.GetValue<bool>("TypeORMSettings:Logging");
    }

    public bool GetTypeORMMigrationsRun()
    {
        return _configuration.GetValue<bool>("TypeORMSettings:MigrationsRun");
    }
    #endregion
    
    #region JWT
    
    public string GetJwtSecret()
    {
        return _configuration.GetValue<string>("TokenSettings:ACCESS_TOKEN_SECRET");
    }
    public DateTime GetJwtExpirationTime()
    {
        return _configuration.GetValue<DateTime>("TokenSettings:ACCESS_TOKEN_EXPIRES_IN");
    }
    public string GetJwtRefreshSecret()
    {
        return _configuration.GetValue<string>("TokenSettings:REFRESH_TOKEN_SECRET");
    }
    public string GetJwtRefreshCookieKey()
    {
        return _configuration.GetValue<string>("TokenSettings:REFRESH_TOKEN_COOKIE_KEY");
    }
    public DateTime GetJwtRefreshExpirationTime()
    {
        return _configuration.GetValue<DateTime>("TokenSettings:REFRESH_TOKEN_EXPIRES_IN");
    }
    public string GetJwtRefreshTokenCookieMaxAge()
    {
        return _configuration.GetValue<string>("TokenSettings:REFRESH_TOKEN_COOKIE_MAX_AGE");
    }
    public string GetJwtType()
    {
        return _configuration.GetValue<string>("TokenSettings:TOKEN_TYPE");
    }
    public DateTime GetJwtPasswordExpirationTime()
    {
        return _configuration.GetValue<DateTime>("TokenSettings:RESET_PASSWORD_LINK_EXPIRES_IN");
    }
    public string GetJwtPasswordSecret()
    {
        return _configuration.GetValue<string>("TokenSettings:RESET_PASSWORD_SECRET");
    }
    
    #endregion
}