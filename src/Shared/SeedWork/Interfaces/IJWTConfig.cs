namespace Shared.SeedWork.Interfaces;

/// <summary>
/// Defines the configuration settings required for JWT (JSON Web Token) handling.
/// </summary>
public interface IJWTConfig
{
    /// <summary>
    /// Gets the secret key used to sign JWTs.
    /// </summary>
    /// <returns>The secret key for JWT signing.</returns>
    string? GetJwtSecret();

    /// <summary>
    /// Gets the expiration time (in minutes) for the JWT.
    /// </summary>
    /// <returns>The expiration time for JWT in minutes.</returns>
    int GetJwtExpirationTime();

    /// <summary>
    /// Gets the secret key used to sign refresh tokens.
    /// </summary>
    /// <returns>The secret key for signing refresh tokens.</returns>
    string? GetJwtRefreshSecret();

    /// <summary>
    /// Gets the key used to store the refresh token in cookies.
    /// </summary>
    /// <returns>The key for storing the refresh token in cookies.</returns>
    string? GetJwtRefreshCookieKey();

    /// <summary>
    /// Gets the expiration time (in minutes) for the refresh token.
    /// </summary>
    /// <returns>The expiration time for refresh tokens in minutes.</returns>
    int GetJwtRefreshExpirationTime();

    /// <summary>
    /// Gets the maximum age for the refresh token cookie.
    /// </summary>
    /// <returns>The max age for the refresh token cookie as a string (e.g., "30d", "1h").</returns>
    string? GetJwtRefreshTokenCookieMaxAge();

    /// <summary>
    /// Gets the type or category of the JWT (e.g., "Bearer").
    /// </summary>
    /// <returns>The type or category of the JWT.</returns>
    string? GetJwtType();

    /// <summary>
    /// Gets the expiration time (in minutes) for the JWT password reset token.
    /// </summary>
    /// <returns>The expiration time for password reset tokens in minutes.</returns>
    int GetJwtPasswordExpirationTime();

    /// <summary>
    /// Gets the secret key used to sign JWT password reset tokens.
    /// </summary>
    /// <returns>The secret key for signing password reset tokens.</returns>
    string? GetJwtPasswordSecret();
}