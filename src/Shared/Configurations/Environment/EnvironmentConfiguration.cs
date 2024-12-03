using Microsoft.Extensions.Configuration;
using Shared.SeedWork.Interfaces;

namespace Shared.Configurations.Environment;

public class EnvironmentConfiguration
    : IDatabaseConfig, IJWTConfig, IIdentityServer, IOAuth, ISmtpConfig
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
    #region SMTP
    public string GetSmtpServer() => 
        _configuration.GetValue<string>("SMTP:SmtpServer");
    public string GetSmtpPort() => 
        _configuration.GetValue<string>("SMTP:SmtpPort");
    public string GetSmtpUsername() => 
        _configuration.GetValue<string>("SMTP:SmtpUsername");
    public string GetSmtpPassword() => 
        _configuration.GetValue<string>("SMTP:SmtpPassword");
    public string GetSmtpEmail() => 
        _configuration.GetValue<string>("SMTP:FromEmail");
    #endregion
    
    #region OAuth
    public string GetGoogleClientId() => 
        _configuration.GetValue<string>("OAuth:ClientId");

    public string GetGoogleClientSecret() => 
        _configuration.GetValue<string>("OAuth:ClientSecret");
    #endregion
    
    #region IdentityServer

    public string GetIdentityServerAuthority() => 
        _configuration.GetValue<string>("IdentityServer:Authority");
    public string GetIdentityServerClientId() => 
        _configuration.GetValue<string>("IdentityServer:ClientId");
    public string GetIdentityServerClientName() => 
        _configuration.GetValue<string>("IdentityServer:ClientName");
    public string GetIdentityServerClientSecret() => 
        _configuration.GetValue<string>("IdentityServer:ClientSecret");
    public string GetIdentityServerInteractiveClientName() => 
        _configuration.GetValue<string>("IdentityServer:InteractiveClientName");
    public string GetIdentityServerInteractiveClientId() => 
        _configuration.GetValue<string>("IdentityServer:InteractiveClientId");
    public string GetIdentityServerInteractiveClientSecret() => 
        _configuration.GetValue<string>("IdentityServer:InteractiveClientSecret");
    public string GetIdentityServerScopes() => 
        _configuration.GetValue<string>("IdentityServer:Scopes");
    
    #endregion
    
    #region Database
    public string GetDatabaseHost() => 
        _configuration.GetValue<string>("ConnectionStrings:Host");
    public int GetDatabasePort() => 
        _configuration.GetValue<int>("ConnectionStrings:Port");
    public string GetDatabaseName() => 
        _configuration.GetValue<string>("ConnectionStrings:Database");
    public string GetDatabaseUserId() => 
        _configuration.GetValue<string>("ConnectionStrings:UserId");
    public string GetDatabasePassword() => 
        _configuration.GetValue<string>("ConnectionStrings:Password");
    public string GetMultipleActiveResultSets() => 
        _configuration.GetValue<string>("ConnectionStrings:MultipleActiveResultSets");
    public bool GetTrustServerCertificate() => 
        _configuration.GetValue<bool>("ConnectionStrings:TrustServerCertificate");
    public string GetDatabaseSchema() => 
        _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
    #endregion
    
    #region Type ORM
    public bool GetTypeORMSync() => 
        _configuration.GetValue<bool>("TypeORMSettings:Sync");

    public bool GetTypeORMLogging() => 
        _configuration.GetValue<bool>("TypeORMSettings:Logging");

    public bool GetTypeORMMigrationsRun() => 
        _configuration.GetValue<bool>("TypeORMSettings:MigrationsRun");
    #endregion
    
    #region JWT
    
    public string GetJwtSecret() => 
        _configuration.GetValue<string>("TokenSettings:ACCESS_TOKEN_SECRET");
    public int GetJwtExpirationTime() => 
        _configuration.GetValue<int>("TokenSettings:ACCESS_TOKEN_EXPIRES_IN");
    public string GetJwtRefreshSecret() => 
        _configuration.GetValue<string>("TokenSettings:REFRESH_TOKEN_SECRET");
    public string GetJwtRefreshCookieKey() => 
        _configuration.GetValue<string>("FrontEnd:REFRESH_TOKEN_COOKIE_KEY");
    public int GetJwtRefreshExpirationTime() => 
        _configuration.GetValue<int>("TokenSettings:REFRESH_TOKEN_EXPIRES_IN");
    public string GetJwtRefreshTokenCookieMaxAge() => 
        _configuration.GetValue<string>("TokenSettings:REFRESH_TOKEN_COOKIE_MAX_AGE");
    public string GetJwtType() => 
        _configuration.GetValue<string>("TokenSettings:TOKEN_TYPE");
    public int GetJwtPasswordExpirationTime() => 
        _configuration.GetValue<int>("TokenSettings:RESET_PASSWORD_LINK_EXPIRES_IN");
    public string GetJwtPasswordSecret() => 
        _configuration.GetValue<string>("TokenSettings:RESET_PASSWORD_SECRET");
    
    #endregion
}