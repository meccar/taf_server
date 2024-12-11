using Microsoft.Extensions.Configuration;
using Shared.SeedWork.Interfaces;

namespace Shared.Configurations.Environment;

    /// <summary>
    /// Provides configuration values for various system settings including JWT, SMTP, OAuth, IdentityServer, Database, and TypeORM.
    /// This class interacts with the application configuration and retrieves relevant settings.
    /// </summary>
    public class EnvironmentConfiguration
        : IDatabaseConfig, IJWTConfig, IIdentityServer, IOAuth, ISmtpConfig
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentConfiguration"/> class with the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration instance to retrieve settings from.</param>
        public EnvironmentConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region SMTP

        /// <summary>
        /// Gets the SMTP server address from the configuration.
        /// </summary>
        /// <returns>The SMTP server address.</returns>
        public string? GetSmtpServer() => 
            _configuration.GetValue<string>("SMTP:SmtpServer");

        /// <summary>
        /// Gets the SMTP server port from the configuration.
        /// </summary>
        /// <returns>The SMTP server port.</returns>
        public string? GetSmtpPort() => 
            _configuration.GetValue<string>("SMTP:SmtpPort");

        /// <summary>
        /// Gets the SMTP username from the configuration.
        /// </summary>
        /// <returns>The SMTP username.</returns>
        public string? GetSmtpUsername() => 
            _configuration.GetValue<string>("SMTP:SmtpUsername");

        /// <summary>
        /// Gets the SMTP password from the configuration.
        /// </summary>
        /// <returns>The SMTP password.</returns>
        public string? GetSmtpPassword() => 
            _configuration.GetValue<string>("SMTP:SmtpPassword");

        /// <summary>
        /// Gets the sender's email address from the configuration.
        /// </summary>
        /// <returns>The sender's email address.</returns>
        public string? GetSmtpEmail() => 
            _configuration.GetValue<string>("SMTP:FromEmail");

        #endregion

        #region OAuth

        /// <summary>
        /// Gets the Google OAuth client ID from the configuration.
        /// </summary>
        /// <returns>The Google OAuth client ID.</returns>
        public string? GetGoogleClientId() => 
            _configuration.GetValue<string>("OAuth:ClientId");

        /// <summary>
        /// Gets the Google OAuth client secret from the configuration.
        /// </summary>
        /// <returns>The Google OAuth client secret.</returns>
        public string? GetGoogleClientSecret() => 
            _configuration.GetValue<string>("OAuth:ClientSecret");

        #endregion

        #region IdentityServer

        /// <summary>
        /// Gets the IdentityServer authority from the configuration.
        /// </summary>
        /// <returns>The IdentityServer authority.</returns>
        public string? GetIdentityServerAuthority() => 
            _configuration.GetValue<string>("IdentityServer:Authority");

        /// <summary>
        /// Gets the IdentityServer client ID from the configuration.
        /// </summary>
        /// <returns>The IdentityServer client ID.</returns>
        public string? GetIdentityServerClientId() => 
            _configuration.GetValue<string>("IdentityServer:ClientId");

        /// <summary>
        /// Gets the IdentityServer client name from the configuration.
        /// </summary>
        /// <returns>The IdentityServer client name.</returns>
        public string? GetIdentityServerClientName() => 
            _configuration.GetValue<string>("IdentityServer:ClientName");

        /// <summary>
        /// Gets the IdentityServer client secret from the configuration.
        /// </summary>
        /// <returns>The IdentityServer client secret.</returns>
        public string? GetIdentityServerClientSecret() => 
            _configuration.GetValue<string>("IdentityServer:ClientSecret");

        /// <summary>
        /// Gets the IdentityServer interactive client name from the configuration.
        /// </summary>
        /// <returns>The IdentityServer interactive client name.</returns>
        public string? GetIdentityServerInteractiveClientName() => 
            _configuration.GetValue<string>("IdentityServer:InteractiveClientName");

        /// <summary>
        /// Gets the IdentityServer interactive client ID from the configuration.
        /// </summary>
        /// <returns>The IdentityServer interactive client ID.</returns>
        public string? GetIdentityServerInteractiveClientId() => 
            _configuration.GetValue<string>("IdentityServer:InteractiveClientId");

        /// <summary>
        /// Gets the IdentityServer interactive client secret from the configuration.
        /// </summary>
        /// <returns>The IdentityServer interactive client secret.</returns>
        public string? GetIdentityServerInteractiveClientSecret() => 
            _configuration.GetValue<string>("IdentityServer:InteractiveClientSecret");

        /// <summary>
        /// Gets the IdentityServer scopes from the configuration.
        /// </summary>
        /// <returns>The IdentityServer scopes.</returns>
        public string? GetIdentityServerScopes() => 
            _configuration.GetValue<string>("IdentityServer:Scopes");

        #endregion

        #region Database

        /// <summary>
        /// Gets the database host from the configuration.
        /// </summary>
        /// <returns>The database host address.</returns>
        public string? GetDatabaseHost() => 
            _configuration.GetValue<string>("ConnectionStrings:Host");

        /// <summary>
        /// Gets the database port from the configuration.
        /// </summary>
        /// <returns>The database port number.</returns>
        public int GetDatabasePort() => 
            _configuration.GetValue<int>("ConnectionStrings:Port");

        /// <summary>
        /// Gets the database name from the configuration.
        /// </summary>
        /// <returns>The database name.</returns>
        public string? GetDatabaseName() => 
            _configuration.GetValue<string>("ConnectionStrings:Database");

        /// <summary>
        /// Gets the database user ID from the configuration.
        /// </summary>
        /// <returns>The database user ID.</returns>
        public string? GetDatabaseUserId() => 
            _configuration.GetValue<string>("ConnectionStrings:UserId");

        /// <summary>
        /// Gets the database password from the configuration.
        /// </summary>
        /// <returns>The database password.</returns>
        public string? GetDatabasePassword() => 
            _configuration.GetValue<string>("ConnectionStrings:Password");

        /// <summary>
        /// Gets the "MultipleActiveResultSets" setting from the configuration.
        /// </summary>
        /// <returns>The "MultipleActiveResultSets" setting value.</returns>
        public string? GetMultipleActiveResultSets() => 
            _configuration.GetValue<string>("ConnectionStrings:MultipleActiveResultSets");

        /// <summary>
        /// Gets the "TrustServerCertificate" setting from the configuration.
        /// </summary>
        /// <returns>The "TrustServerCertificate" setting value.</returns>
        public bool GetTrustServerCertificate() => 
            _configuration.GetValue<bool>("ConnectionStrings:TrustServerCertificate");

        /// <summary>
        /// Gets the default database schema from the configuration.
        /// </summary>
        /// <returns>The database schema.</returns>
        public string? GetDatabaseSchema() => 
            _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");

        #endregion

        #region Type ORM

        /// <summary>
        /// Gets the sync setting for TypeORM from the configuration.
        /// </summary>
        /// <returns>The TypeORM sync setting.</returns>
        public bool GetTypeOrmSync() => 
            _configuration.GetValue<bool>("TypeORMSettings:Sync");

        /// <summary>
        /// Gets the logging setting for TypeORM from the configuration.
        /// </summary>
        /// <returns>The TypeORM logging setting.</returns>
        public bool GetTypeOrmLogging() => 
            _configuration.GetValue<bool>("TypeORMSettings:Logging");

        /// <summary>
        /// Gets the migrations run setting for TypeORM from the configuration.
        /// </summary>
        /// <returns>The TypeORM migrations run setting.</returns>
        public bool GetTypeOrmMigrationsRun() => 
            _configuration.GetValue<bool>("TypeORMSettings:MigrationsRun");

        #endregion

        #region JWT

        /// <summary>
        /// Gets the JWT secret from the configuration.
        /// </summary>
        /// <returns>The JWT secret key.</returns>
        public string? GetJwtSecret() => 
            _configuration.GetValue<string>("TokenSettings:ACCESS_TOKEN_SECRET");

        /// <summary>
        /// Gets the JWT expiration time from the configuration.
        /// </summary>
        /// <returns>The JWT expiration time in seconds.</returns>
        public int GetJwtExpirationTime() => 
            _configuration.GetValue<int>("TokenSettings:ACCESS_TOKEN_EXPIRES_IN");

        /// <summary>
        /// Gets the JWT refresh token secret from the configuration.
        /// </summary>
        /// <returns>The JWT refresh token secret key.</returns>
        public string? GetJwtRefreshSecret() => 
            _configuration.GetValue<string>("TokenSettings:REFRESH_TOKEN_SECRET");

        /// <summary>
        /// Gets the JWT refresh token cookie key from the configuration.
        /// </summary>
        /// <returns>The JWT refresh token cookie key.</returns>
        public string? GetJwtRefreshCookieKey() => 
            _configuration.GetValue<string>("FrontEnd:REFRESH_TOKEN_COOKIE_KEY");

        /// <summary>
        /// Gets the JWT refresh token expiration time from the configuration.
        /// </summary>
        /// <returns>The JWT refresh token expiration time in seconds.</returns>
        public int GetJwtRefreshExpirationTime() => 
            _configuration.GetValue<int>("TokenSettings:REFRESH_TOKEN_EXPIRES_IN");

        /// <summary>
        /// Gets the JWT refresh token cookie max age from the configuration.
        /// </summary>
        /// <returns>The JWT refresh token cookie max age.</returns>
        public string? GetJwtRefreshTokenCookieMaxAge() => 
            _configuration.GetValue<string>("TokenSettings:REFRESH_TOKEN_COOKIE_MAX_AGE");

        /// <summary>
        /// Gets the JWT token type from the configuration.
        /// </summary>
        /// <returns>The JWT token type.</returns>
        public string? GetJwtType() => 
            _configuration.GetValue<string>("TokenSettings:TOKEN_TYPE");

        /// <summary>
        /// Gets the password reset JWT expiration time from the configuration.
        /// </summary>
        /// <returns>The JWT password reset expiration time in seconds.</returns>
        public int GetJwtPasswordExpirationTime() => 
            _configuration.GetValue<int>("TokenSettings:RESET_PASSWORD_LINK_EXPIRES_IN");

        /// <summary>
        /// Gets the JWT password reset secret from the configuration.
        /// </summary>
        /// <returns>The JWT password reset secret key.</returns>
        public string? GetJwtPasswordSecret() => 
            _configuration.GetValue<string>("TokenSettings:RESET_PASSWORD_SECRET");

        #endregion
    }