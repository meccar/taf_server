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
            new Claim("EId", user.EId),
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

// using IdentityModel.Client;
// using Domain.Entities;
// using Domain.Interfaces;
// using Domain.Model;
// using Infrastructure.Configurations.Environment;
// using Microsoft.Extensions.Logging;
//
// namespace Infrastructure.Repositories.Service;
//
// public class TokenService : ITokenService
// {
//     private readonly HttpClient _httpClient;
//     private readonly EnvironmentConfiguration _environment;
//     private readonly ILogger<TokenService> _logger;
//
//     public TokenService(
//         IHttpClientFactory httpClientFactory,
//         EnvironmentConfiguration environment,
//         ILogger<TokenService> logger)
//     {
//         _httpClient = httpClientFactory.CreateClient();
//         _environment = environment;
//         _logger = logger;
//     }
//
//     public async Task<UserTokenModel> GenerateTokenPair(UserLoginDataEntity user)
//     {
//         // Discover IdentityServer endpoints
//         var disco = await _httpClient.GetDiscoveryDocumentAsync(
//             _environment.GetIdentityServerAuthority()
//         );
//         
//         if (disco.IsError)
//         {
//             _logger.LogError("Discovery error: {Error}", disco.Error);
//             throw new Exception("Unable to connect to identity server");
//         }
//
//         // Request token
//         var tokenResponse = await _httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
//         {
//             Address = disco.TokenEndpoint,
//             ClientId = _environment.GetIdentityServerClientId(),
//             ClientSecret = _environment.GetIdentityServerClientSecret(),
//             UserName = user.Email,
//             Password = user.PasswordHash,
//             Scope = "openid profile api1"
//         });
//
//         if (tokenResponse.IsError)
//         {
//             _logger.LogError("Token request error: {Error}", tokenResponse.Error);
//             throw new Exception(tokenResponse.Error);
//         }
//
//         return new UserTokenModel(null, null, null, null)
//         {
//             Token = new TokenModel(
//                 _environment.GetJwtType(),
//                 tokenResponse.AccessToken,
//                 (int)tokenResponse.ExpiresIn,
//                 tokenResponse.RefreshToken,
//                 (int)_environment.GetJwtRefreshExpirationTime() * 3600 // Convert hours to seconds
//             )
//         };
//     }
//
//     public async Task<UserTokenModel> RefreshToken(string refreshToken)
//     {
//         var disco = await _httpClient.GetDiscoveryDocumentAsync(
//             _environment.GetIdentityServerAuthority()
//         );
//
//         if (disco.IsError)
//         {
//             throw new Exception(disco.Error);
//         }
//
//         var tokenResponse = await _httpClient.RequestRefreshTokenAsync(new RefreshTokenRequest
//         {
//             Address = disco.TokenEndpoint,
//             ClientId = _environment.GetIdentityServerClientId(),
//             ClientSecret = _environment.GetIdentityServerClientSecret(),
//             RefreshToken = refreshToken
//         });
//
//         if (tokenResponse.IsError)
//         {
//             throw new Exception(tokenResponse.Error);
//         }
//
//         return new UserTokenModel(null, null, null, null)
//         {
//             Token = new TokenModel(
//                 _environment.GetJwtType(),
//                 tokenResponse.AccessToken,
//                 (int)tokenResponse.ExpiresIn,
//                 tokenResponse.RefreshToken,
//                 (int)_environment.GetJwtRefreshExpirationTime() * 3600
//             )
//         };
//     }
// }