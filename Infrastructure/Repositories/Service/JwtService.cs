using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Interfaces;
using Domain.Model;
using Domain.SeedWork.Enums.Token;
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
        var accessTokenExpires = _environmentConfiguration.GetJwtExpirationTime();
        var refreshTokenExpires = _environmentConfiguration.GetJwtRefreshExpirationTime();
        var accessToken = await GenerateJwtAccessToken(payload, accessTokenExpires);
        var refreshToken = await GenerateJwtRefreshToken(payload, refreshTokenExpires);

        return new TokenModel
        (
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

    private async Task<string>
        GenerateJwtAccessToken
        (ClaimsIdentity payload, string accessTokenExpires)
    {
        var handler = new JwtSecurityTokenHandler();
        
        var accessTokenSecret = Encoding.UTF8.GetBytes(_environmentConfiguration.GetJwtSecret());

        var accessTokenCredentials = new SigningCredentials(
            new SymmetricSecurityKey(accessTokenSecret),
            SecurityAlgorithms.HmacSha512);
        
        DateTime.TryParse(accessTokenExpires, out var datetimeTokenExpires);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = accessTokenCredentials,
            Expires = datetimeTokenExpires,
            Subject = payload
        };
        
        return handler.WriteToken(handler.CreateToken(tokenDescriptor));
    }

    private async Task<string> 
        GenerateJwtRefreshToken
        (ClaimsIdentity payload, string refreshTokenExpires)
    {
        var handler = new JwtSecurityTokenHandler();
        
        var refreshTokenSecret = Encoding.UTF8.GetBytes(_environmentConfiguration.GetJwtSecret());
        
        var accessTokenCredentials = new SigningCredentials(
            new SymmetricSecurityKey(refreshTokenSecret),
            SecurityAlgorithms.HmacSha512);
        
        DateTime.TryParse(refreshTokenExpires, out var datetimeTokenExpires);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = accessTokenCredentials,
            Expires = datetimeTokenExpires,
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
            .GetUserTokensByUserAccountId(user.UserAccountId);
        
        if (userToken == null)
        {
            var accessToken = await _unitOfWork
                .UserTokenCommandRepository
                .CreateUserTokenAsync(
                    new UserTokenModel(
                        user.UserAccountId,
                        UserTokenType.Access,
                        "JWT", 
                        token.AccessToken
                        ), user
                    );
            
            var refreshToken = await _unitOfWork
                .UserTokenCommandRepository
                .CreateUserTokenAsync(
                    new UserTokenModel(
                        user.UserAccountId,
                        UserTokenType.Refresh,
                        "JWT",
                        token.RefreshToken
                        ), user
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
                        UserTokenType.Access,
                        "JWT", 
                        token.AccessToken
                    )
                );
            
            var refreshToken = await _unitOfWork
                .UserTokenCommandRepository
                .UpdateUserTokenAsync(
                    new UserTokenModel(
                        user.UserAccountId,
                        UserTokenType.Refresh,
                        "JWT",
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