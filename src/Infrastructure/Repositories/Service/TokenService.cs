using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Model;
using IdentityModel;
using Infrastructure.Configurations.Environment;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Repositories.Service;

public class TokenService : ITokenService
{
    private readonly byte[] _secret;
    private readonly JwtSecurityTokenHandler _jwtHandler;
    private readonly EnvironmentConfiguration _environment;
    public TokenService
    (
        EnvironmentConfiguration environment
    )
    {
        _secret = Encoding.UTF8.GetBytes(environment.GetJwtSecret());
        _jwtHandler = new JwtSecurityTokenHandler();
        _environment = environment;
    }
    public async Task<UserTokenModel> GenerateTokenPair(UserLoginDataEntity user)
    {
        var claims = await CreateUserClaims(user);
        var tokenType = _environment.GetJwtType();
        
        var (accessToken, accessExpiration) = await CreateToken(user, claims, _environment.GetJwtExpirationTime());
        var (refreshToken, refreshExpiration) = await CreateToken(user, claims, _environment.GetJwtRefreshExpirationTime());

        return new UserTokenModel(null, null, null, null, claims)
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
    
    private async Task<List<Claim>> CreateUserClaims(UserLoginDataEntity user)
    {
        // var systemInfo = GetSystemInfo();
        // var localIpAddress = GetLocalIPAddress();

        return new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.EId),
            new Claim(ClaimTypes.Email, user.Email),
            // new Claim("SystemInfo", systemInfo),
            // new Claim("LocalIPAddress", localIpAddress)
        };
    }
    
    private async Task<(string token, TimeSpan expiration)> CreateToken(UserLoginDataEntity user, List<Claim> claims, double expirationTimeInHours)
    {
        var expiration = TimeSpan.FromHours(expirationTimeInHours);
        var token = await GenerateToken(user, claims, expiration);
        return (token, expiration);
    }

    private async Task<string> GenerateToken(UserLoginDataEntity user, List<Claim> claims, TimeSpan expiration)
    {
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(_secret),
            SecurityAlgorithms.HmacSha256);
        
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.Add(expiration)
        };

        var token = _jwtHandler.CreateToken(descriptor);
        return _jwtHandler.WriteToken(token);
    }
}