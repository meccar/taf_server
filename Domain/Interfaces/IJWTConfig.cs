namespace Domain.Interfaces;

public interface IJWTConfig
{
    string GetJwtSecret();
    DateTime GetJwtExpirationTime();
    string GetJwtRefreshSecret();
    string GetJwtRefreshCookieKey();
    DateTime GetJwtRefreshExpirationTime();
    string GetJwtRefreshTokenCookieMaxAge();
    string GetJwtType();
    DateTime GetJwtPasswordExpirationTime();
    string GetJwtPasswordSecret();
}