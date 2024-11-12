using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Interfaces;
using Domain.Model;
using Domain.SeedWork.Enums.Token;
using Domain.SeedWork.Enums.UserLoginDataExternal;
using Infrastructure.Configurations.Environment;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Repositories.Service;

public class JwtService : IJwtService
{
    private readonly EnvironmentConfiguration _environmentConfiguration;
    private readonly IUnitOfWork _unitOfWork;

    public JwtService(
        EnvironmentConfiguration environmentConfiguration,
        IUnitOfWork unitOfWork
        )
    {
        _environmentConfiguration = environmentConfiguration;
        _unitOfWork = unitOfWork;
    }
    
    private async Task<TokenModel>
        GenerateTokens(UserLoginDataModel user)
    {
        var payload = GenerateClaims(user);
        var tokenType = _environmentConfiguration.GetJwtType();
        
        var accessTokenExpires = TimeSpan.FromHours(_environmentConfiguration.GetJwtExpirationTime());
        var accessToken = await GenerateJwtAccessToken(payload, accessTokenExpires);
        
        var refreshTokenExpires = TimeSpan.FromHours(_environmentConfiguration.GetJwtRefreshExpirationTime());
        var refreshToken = await GenerateJwtRefreshToken(payload, refreshTokenExpires);

        return new TokenModel
        (
            tokenType,
            accessToken,
            accessTokenExpires.ToString(),
            refreshToken,
            refreshTokenExpires.ToString()
        );
    }
    
    private static ClaimsIdentity 
        GenerateClaims
        (UserLoginDataModel user)
    {
        var ci = new ClaimsIdentity();

        ci.AddClaim(new Claim("id", user.UserAccountId));
        ci.AddClaim(new Claim(ClaimTypes.Email, user.Email));
        // ci.AddClaim(new Claim(ClaimTypes.GivenName, user.UserAccount.FirstName));
        // ci.AddClaim(new Claim(ClaimTypes.Surname, user.UserAccount.LastName));
    
        return ci;
    }

    private async Task<string>
        GenerateJwtAccessToken
        (ClaimsIdentity payload, TimeSpan accessTokenExpires)
    {
        var handler = new JwtSecurityTokenHandler();
        
        var accessTokenSecret = Encoding.UTF8.GetBytes(_environmentConfiguration.GetJwtSecret());

        var accessTokenCredentials = new SigningCredentials(
            new SymmetricSecurityKey(accessTokenSecret),
            SecurityAlgorithms.HmacSha256);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = accessTokenCredentials,
            Expires = DateTime.Now.Add(accessTokenExpires),
            Subject = payload
        };
        
        return handler.WriteToken(handler.CreateToken(tokenDescriptor));
    }

    private async Task<string> 
        GenerateJwtRefreshToken
        (ClaimsIdentity payload, TimeSpan refreshTokenExpires)
    {
        var handler = new JwtSecurityTokenHandler();
        
        var refreshTokenSecret = Encoding.UTF8.GetBytes(_environmentConfiguration.GetJwtSecret());
        
        var accessTokenCredentials = new SigningCredentials(
            new SymmetricSecurityKey(refreshTokenSecret),
            SecurityAlgorithms.HmacSha256);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = accessTokenCredentials,
            Expires = DateTime.Now.Add(refreshTokenExpires),
            Subject = payload
        };
        
        return handler.WriteToken(handler.CreateToken(tokenDescriptor));
    }
    
    private async Task 
        UpdateOrCreateTokens
        (UserLoginDataModel user, TokenModel token)
    {
        var userToken = await _unitOfWork
            .UserTokenCommandRepository
            .TokenExistsAsync(user.Id, token);
        
        if (userToken == null)
        {
            var accessToken = await _unitOfWork
                .UserTokenCommandRepository
                .CreateUserTokenAsync(
                    new UserTokenModel(
                        user.UserAccountId,
                        ETokenName.ACCESS,
                        EProvider.PASSWORD, 
                        token.AccessToken
                        )
                    );
            
            var refreshToken = await _unitOfWork
                .UserTokenCommandRepository
                .CreateUserTokenAsync(
                    new UserTokenModel(
                        user.UserAccountId,
                        ETokenName.REFRESH,
                        EProvider.PASSWORD,
                        token.RefreshToken
                        )
                    );

            if (accessToken == null || refreshToken == null)
                throw new Exception();
        }
        else
        {
            if (!string.IsNullOrEmpty(userToken[0].Value))
            {
                // Todo blacklist
                // await _unitOfWork.GenerateBlacklistToken
            }

            var accessToken = await _unitOfWork
                .UserTokenCommandRepository
                .UpdateUserTokenAsync(
                    new UserTokenModel(
                        user.UserAccountId,
                        ETokenName.ACCESS,
                        EProvider.PASSWORD, 
                        token.AccessToken
                    )
                );
            
            var refreshToken = await _unitOfWork
                .UserTokenCommandRepository
                .UpdateUserTokenAsync(
                    new UserTokenModel(
                        user.UserAccountId,
                        ETokenName.REFRESH,
                        EProvider.PASSWORD,
                        token.RefreshToken
                    )
                );
            
            if (accessToken == null || refreshToken == null)
                throw new Exception();
        }
    }

    private string GetCookieWithJwtRefreshToken(string refreshToken)
    {
        var refreshTokenValue = _environmentConfiguration.GetJwtRefreshCookieKey();
        var refreshExpirationTime = _environmentConfiguration.GetJwtRefreshTokenCookieMaxAge();
        
        return $"{refreshTokenValue}={refreshToken}; HttpOnly; SameSite=None; Secure; Path=/; Max-Age={refreshExpirationTime};";
    }
    
    public async Task<TokenModel> 
        ResponseAuthWithAccessTokenAndRefreshTokenCookie
        (UserLoginDataModel user)
    {
        var token = await GenerateTokens(user);
        
        await UpdateOrCreateTokens(user, token);
        var refreshTokenCookie = GetCookieWithJwtRefreshToken(token.RefreshToken);
        
        return new TokenModel(
            token.TokenType,
            token.AccessToken,
            token.AccessTokenExpires,
            token.RefreshToken,
            token.RefreshTokenExpires);
    }
}