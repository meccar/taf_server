using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Aggregates;
using Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Shared.Configurations.Environment;
using Shared.Model;

namespace Infrastructure.Repositories;

public class TokenRepository : ITokenRepository
{
    private readonly byte[] _secret;
    private readonly JwtSecurityTokenHandler _jwtHandler;
    private readonly EnvironmentConfiguration _environment;
    public TokenRepository
    (
        EnvironmentConfiguration environment
    )
    {
        _secret = Encoding.UTF8.GetBytes(environment.GetJwtSecret());
        _jwtHandler = new JwtSecurityTokenHandler();
        _environment = environment;
    }
    public async Task<UserTokenModel> GenerateTokenPair(UserAccountAggregate user)
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
    
    private async Task<List<Claim>> CreateUserClaims(UserAccountAggregate user)
    {
        return new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.EId),
            new Claim(ClaimTypes.Email, user.Email),
        };
    }
    
    private async Task<(string token, TimeSpan expiration)> CreateToken(UserAccountAggregate user, List<Claim> claims, double expirationTimeInHours)
    {
        var expiration = TimeSpan.FromHours(expirationTimeInHours);
        var token = await GenerateToken(user, claims, expiration);
        return (token, expiration);
    }

    private async Task<string> GenerateToken(UserAccountAggregate user, List<Claim> claims, TimeSpan expiration)
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