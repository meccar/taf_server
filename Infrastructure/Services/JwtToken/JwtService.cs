using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Interfaces;
using Domain.Model;
using Infrastructure.Configurations.Environment;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services.JwtToken;

public class JwtService : IJwtService
{
    private readonly EnvironmentConfiguration _environmentConfiguration;
    private readonly IUnitOfWork _unitOfWork;

    public JwtService(
        EnvironmentConfiguration environmentConfiguration,
        IUnitOfWork unitOfWork
        )
    {
        _unitOfWork = unitOfWork;
        _environmentConfiguration = environmentConfiguration;
    }
    private async Task<(
            string tokenType, 
            SecurityToken accessToken, 
            DateTime accessTokenExpires, 
            SecurityToken refreshToken, 
            DateTime refreshTokenExpires)>
        GenerateTokens(UserLoginDataModel user)
    {
        var payload = GenerateClaims(user);
        var tokenType = _environmentConfiguration.GetJwtType();
        var accessTokenExpires = _environmentConfiguration.GetJwtExpirationTime();
        var refreshTokenExpires = _environmentConfiguration.GetJwtRefreshExpirationTime();
        var accessToken = await GenerateJwtToken(payload, accessTokenExpires);
        var refreshToken = await GenerateJwtRefreshToken(payload, refreshTokenExpires);
            
        return (
            tokenType,
            accessToken,
            accessTokenExpires,
            refreshToken,
            refreshTokenExpires
        );
    }
    
    private static ClaimsIdentity 
        GenerateClaims
        (UserLoginDataModel user)
    {
        var ci = new ClaimsIdentity();

        ci.AddClaim(new Claim("id", user.Id.ToString()));
        ci.AddClaim(new Claim(ClaimTypes.Email, user.Email));
        ci.AddClaim(new Claim(ClaimTypes.GivenName, user.UserAccount.FirstName));
        ci.AddClaim(new Claim(ClaimTypes.Surname, user.UserAccount.LastName));
    
        return ci;
    }

    private async Task<SecurityToken>
        GenerateJwtToken
        (ClaimsIdentity payload, DateTime accessTokenExpires)
    {
        var handler = new JwtSecurityTokenHandler();
        
        var accessTokenSecret = Encoding.UTF8.GetBytes(_environmentConfiguration.GetJwtSecret());

        var accessTokenCredentials = new SigningCredentials(
            new SymmetricSecurityKey(accessTokenSecret),
            SecurityAlgorithms.HmacSha512);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = accessTokenCredentials,
            Expires = accessTokenExpires,
            Subject = payload
        };
        
        return handler.CreateToken(tokenDescriptor);
    }

    private async Task<SecurityToken> 
        GenerateJwtRefreshToken
        (ClaimsIdentity payload, DateTime refreshTokenExpires)
    {
        var handler = new JwtSecurityTokenHandler();
        
        var refreshTokenSecret = Encoding.UTF8.GetBytes(_environmentConfiguration.GetJwtSecret());
        
        var accessTokenCredentials = new SigningCredentials(
            new SymmetricSecurityKey(refreshTokenSecret),
            SecurityAlgorithms.HmacSha512);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = accessTokenCredentials,
            Expires = refreshTokenExpires,
            Subject = payload
        };
        
        return handler.CreateToken(tokenDescriptor);
    }

    private async Task 
        UpdateOrCreateTokens
        (UserLoginDataModel user, TokenModel token)
    {
        await _unitOfWork.UserTokenCommandRepository.CreateUserTokenAsync(token);
    }
    
    public async Task<string> 
        ResponseAuthWithAccessTokenAndRefreshTokenCookie
        (UserLoginDataModel user)
    {
        var token = await GenerateTokens(user);
        return null;
    }
}