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
    
    // #region JWT
    // public string GetJwtSecret()
    // {
    //     return _configuration.GetValue<string>("TokenSettings:ACCESS_TOKEN_SECRET");
    // }
    // #endregion
    
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
    public string GetJwtExpirationTime()
    {
        return _configuration.GetValue<string>("TokenSettings:ACCESS_TOKEN_EXPIRES_IN");
    }
    public string GetJwtRefreshSecret()
    {
        return _configuration.GetValue<string>("TokenSettings:REFRESH_TOKEN_SECRET");
    }
    public string GetJwtRefreshCookieKey()
    {
        return _configuration.GetValue<string>("FrontEnd:REFRESH_TOKEN_COOKIE_KEY");
    }
    public string GetJwtRefreshExpirationTime()
    {
        return _configuration.GetValue<string>("TokenSettings:REFRESH_TOKEN_EXPIRES_IN");
    }
    public string GetJwtRefreshTokenCookieMaxAge()
    {
        return _configuration.GetValue<string>("TokenSettings:REFRESH_TOKEN_COOKIE_MAX_AGE");
    }
    public string GetJwtType()
    {
        return _configuration.GetValue<string>("TokenSettings:TOKEN_TYPE");
    }
    public string GetJwtPasswordExpirationTime()
    {
        return _configuration.GetValue<string>("TokenSettings:RESET_PASSWORD_LINK_EXPIRES_IN");
    }
    public string GetJwtPasswordSecret()
    {
        return _configuration.GetValue<string>("TokenSettings:RESET_PASSWORD_SECRET");
    }
    
    #endregion
}