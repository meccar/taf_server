using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Model;
using Infrastructure.Configurations.Environment;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Repositories.Service;

public class TokenService : ITokenService
{
    private readonly JwtSecurityTokenHandler _jwtHandler;
    private readonly byte[] _secret;
    private readonly EnvironmentConfiguration _environment;
    public TokenService
    (
        EnvironmentConfiguration environment
    )
    {
        _jwtHandler = new JwtSecurityTokenHandler();
        _secret = Encoding.UTF8.GetBytes(environment.GetJwtSecret());
        _environment = environment;
    }
    public async Task<UserTokenModel> GenerateTokenPair(UserLoginDataEntity user)
    {
        var claims = await CreateUserClaims(user);
        var tokenType = _environment.GetJwtType();
        
        var (accessToken, accessExpiration) = await CreateToken(claims, _environment.GetJwtExpirationTime());
        var (refreshToken, refreshExpiration) = await CreateToken(claims, _environment.GetJwtRefreshExpirationTime());

        return new UserTokenModel(null, null, null, null)
        {
            Token = new TokenModel(
                tokenType,
                accessToken,
                (int)accessExpiration.TotalSeconds,
                refreshToken,
                (int)refreshExpiration.TotalSeconds
            )
        };
    }
    
    private async Task<ClaimsIdentity> CreateUserClaims(UserLoginDataEntity user)
    {
        // var systemInfo = GetSystemInfo();
        // var localIpAddress = GetLocalIPAddress();

        return new ClaimsIdentity(new[]
        {
            new Claim("id", user.UserAccountId),
            new Claim(ClaimTypes.Email, user.Email),
            // new Claim("SystemInfo", systemInfo),
            // new Claim("LocalIPAddress", localIpAddress)
        });
    }
    
    private async Task<(string token, TimeSpan expiration)> CreateToken(ClaimsIdentity claims, double expirationTimeInHours)
    {
        var expiration = TimeSpan.FromHours(expirationTimeInHours);
        var token = await GenerateToken(claims, expiration);
        return (token, expiration);
    }

    private async Task<string> GenerateToken(ClaimsIdentity claims, TimeSpan expiration)
    {
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(_secret),
            SecurityAlgorithms.HmacSha256);

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.Add(expiration)
        };

        var token = _jwtHandler.CreateToken(descriptor);
        return _jwtHandler.WriteToken(token);
    }
}