using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Aggregates;
using Domain.Interfaces;
using Domain.Interfaces.Tokens;
using Microsoft.IdentityModel.Tokens;
using Shared.Configurations.Environment;
using Shared.Model;

namespace Persistance.Repositories.Tokens;

/// <summary>
/// Repository for generating and managing tokens.
/// </summary>
public class TokenRepository : ITokenRepository
{
    private readonly byte[] _secret;
    private readonly JwtSecurityTokenHandler _jwtHandler;
    private readonly EnvironmentConfiguration _environment;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="TokenRepository"/> class.
    /// </summary>
    /// <param name="environment">The environment configuration.</param>
    public TokenRepository
    (
        EnvironmentConfiguration environment
    )
    {
        _secret = Encoding.UTF8.GetBytes(environment.GetJwtSecret()!);
        _jwtHandler = new JwtSecurityTokenHandler();
        _environment = environment;
    }
    
    /// <summary>
    /// Generates a token pair (access and refresh tokens) for the specified user.
    /// </summary>
    /// <param name="user">The user account aggregate.</param>
    /// <returns>A <see cref="UserTokenModel"/> containing the token pair and related information.</returns>
    public UserTokenModel GenerateTokenPair(UserAccountAggregate user)
    {
        var claims = CreateUserClaims(user);
        var tokenType = _environment.GetJwtType();
        
        var (accessToken, accessExpiration) = CreateToken(claims, _environment.GetJwtExpirationTime());
        var (refreshToken, refreshExpiration) = CreateToken(claims, _environment.GetJwtRefreshExpirationTime());

        return new UserTokenModel(null, null, null, null, claims)
        {
            Token = new TokenModel(
                tokenType!,
                accessToken,
                (int)accessExpiration.TotalSeconds,
                refreshToken,
                (int)refreshExpiration.TotalSeconds
            )
        };
    }
    
    private List<Claim> CreateUserClaims(UserAccountAggregate user)
    {
        return new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.EId),
            new Claim(ClaimTypes.Email, user.Email!),
        };
    }
    
    private (string token, TimeSpan expiration) CreateToken(List<Claim> claims, double expirationTimeInHours)
    {
        var expiration = TimeSpan.FromHours(expirationTimeInHours);
        var token = GenerateToken(claims, expiration);
        return (token, expiration);
    }

    private string GenerateToken(List<Claim> claims, TimeSpan expiration)
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