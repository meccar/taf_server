namespace Shared.SeedWork.Interfaces;

public interface IJWTConfig
{
    string GetJwtSecret();
    int GetJwtExpirationTime();
    string GetJwtRefreshSecret();
    string GetJwtRefreshCookieKey();
    int GetJwtRefreshExpirationTime();
    string GetJwtRefreshTokenCookieMaxAge();
    string GetJwtType();
    int GetJwtPasswordExpirationTime();
    string GetJwtPasswordSecret();
}