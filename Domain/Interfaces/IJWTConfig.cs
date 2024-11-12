namespace Domain.Interfaces;

public interface IJWTConfig
{
    string GetJwtSecret();
    int GetJwtExpirationTime();
    string GetJwtRefreshSecret();
    string GetJwtRefreshCookieKey();
    int GetJwtRefreshExpirationTime();
    string GetJwtRefreshTokenCookieMaxAge();
    string GetJwtType();
    string GetJwtPasswordExpirationTime();
    string GetJwtPasswordSecret();
}