namespace Domain.Interfaces;

/// <summary>
/// Defines the contract for retrieving JWT (JSON Web Token) configuration settings.
/// This interface is used to manage various JWT-related configuration values such as secret keys, expiration times, 
/// and refresh token settings used for authentication and authorization.
/// </summary>
public interface IJWTConfig
{
    /// <summary>
    /// Gets the secret key used for signing JWT tokens.
    /// </summary>
    /// <returns>The JWT secret key.</returns>
    string GetJwtSecret();

    /// <summary>
    /// Gets the expiration time (in minutes) for JWT tokens.
    /// </summary>
    /// <returns>The JWT token expiration time in minutes.</returns>
    int GetJwtExpirationTime();

    /// <summary>
    /// Gets the secret key used for signing JWT refresh tokens.
    /// </summary>
    /// <returns>The JWT refresh token secret key.</returns>
    string GetJwtRefreshSecret();

    /// <summary>
    /// Gets the key used to store the JWT refresh token in the cookies.
    /// </summary>
    /// <returns>The JWT refresh token cookie key.</returns>
    string GetJwtRefreshCookieKey();

    /// <summary>
    /// Gets the expiration time (in minutes) for JWT refresh tokens.
    /// </summary>
    /// <returns>The JWT refresh token expiration time in minutes.</returns>
    int GetJwtRefreshExpirationTime();

    /// <summary>
    /// Gets the max age (as a string) for the JWT refresh token cookie.
    /// </summary>
    /// <returns>The JWT refresh token cookie max age.</returns>
    string GetJwtRefreshTokenCookieMaxAge();

    /// <summary>
    /// Gets the type of JWT token (e.g., bearer token type).
    /// </summary>
    /// <returns>The JWT token type.</returns>
    string GetJwtType();

    /// <summary>
    /// Gets the expiration time (in minutes) for password reset JWT tokens.
    /// </summary>
    /// <returns>The JWT password expiration time in minutes.</returns>
    int GetJwtPasswordExpirationTime();

    /// <summary>
    /// Gets the secret key used for signing JWT tokens related to password resets.
    /// </summary>
    /// <returns>The JWT password reset secret key.</returns>
    string GetJwtPasswordSecret();
}