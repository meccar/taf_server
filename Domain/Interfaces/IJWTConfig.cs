namespace Domain.Interfaces;

public interface IJWTConfig
{
    string GetJwtSecret();
    string GetJwtExpirationTime();
    string GetJwtRefreshSecret();
    string GetJwtRefreshCookieKey();
    string GetJwtRefreshExpirationTime();
    string GetJwtRefreshTokenCookieMaxAge();
    string GetJwtType();
    string GetJwtPasswordExpirationTime();
    string GetJwtPasswordSecret();
}