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
    private readonly EnvironmentConfiguration _environment;
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtSecurityTokenHandler _jwtHandler;
    private readonly byte[] _secret;
    
    public JwtService(
        EnvironmentConfiguration environment,
        IUnitOfWork unitOfWork)
    {
        _environment = environment;
        _unitOfWork = unitOfWork;
        _jwtHandler = new JwtSecurityTokenHandler();
        _secret = Encoding.UTF8.GetBytes(environment.GetJwtSecret());
    }

    public async Task<TokenModel> GenerateAuthResponseWithRefreshTokenCookie(UserLoginDataModel user)
    {
        var token = await GenerateTokenPair(user);
        token.LoginProvider = EProvider.PASSWORD;
        // token.Name = 
        await PersistTokens(user, token);
        
        return new TokenModel(
            token.Token.TokenType,
            token.Token.AccessToken,
            token.Token.AccessTokenExpires,
            token.Token.RefreshToken,
            token.Token.RefreshTokenExpires
            );
    }

    private async Task<UserTokenModel> GenerateTokenPair(UserLoginDataModel user)
    {
        var claims = CreateUserClaims(user);
        var tokenType = _environment.GetJwtType();
        
        var (accessToken, accessExpiration) = await CreateAccessToken(claims);
        var (refreshToken, refreshExpiration) = await CreateRefreshToken(claims);

        return new UserTokenModel(null, null, null)
        {
            Token = new TokenModel(
                tokenType,
                accessToken,
                accessExpiration.ToString(),
                refreshToken,
                refreshExpiration.ToString()
            )
        };
    }
    
    private static ClaimsIdentity CreateUserClaims(UserLoginDataModel user) =>
        new(new[]
        {
            new Claim("id", user.UserAccountId),
            new Claim(ClaimTypes.Email, user.Email)
        });

    private async Task<(string token, TimeSpan expiration)> CreateAccessToken(ClaimsIdentity claims)
    {
        var expiration = TimeSpan.FromHours(_environment.GetJwtExpirationTime());
        
        var token = await GenerateToken(claims, expiration);
        return (token, expiration);
    }

    private async Task<(string token, TimeSpan expiration)> CreateRefreshToken(ClaimsIdentity claims)
    {
        var expiration = TimeSpan.FromHours(_environment.GetJwtRefreshExpirationTime());
        
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
    
    private async Task PersistTokens(UserLoginDataModel user, UserTokenModel token)
    {
        var existingTokens = await _unitOfWork.UserTokenCommandRepository
            .TokenExistsAsync(user.Id, token);

        if (existingTokens == null)
        {
            await CreateNewTokens(user, token);
        }
        else
        {
            await UpdateExistingTokens(user, token, existingTokens);
        }
        
    }

    private async Task CreateNewTokens(UserLoginDataModel user, UserTokenModel token)
    {
        var accessToken = await CreateUserToken(user, ETokenName.ACCESS, token.Token.AccessToken);
        var refreshToken = await CreateUserToken(user, ETokenName.REFRESH, token.Token.RefreshToken);

        if (accessToken == null || refreshToken == null)
        {
            throw new InvalidOperationException("Failed to create user tokens");
        }
    }

    private async Task UpdateExistingTokens(UserLoginDataModel user, UserTokenModel token, object existingTokens)
    {
        // TODO: Implement token blacklisting logic here
        var accessToken = await UpdateUserToken(user, ETokenName.ACCESS, token.Token.AccessToken);
        var refreshToken = await UpdateUserToken(user, ETokenName.REFRESH, token.Token.RefreshToken);

        if (accessToken == null || refreshToken == null)
        {
            throw new InvalidOperationException("Failed to update user tokens");
        }
    }

    private async Task<UserTokenModel?> CreateUserToken(UserLoginDataModel user, ETokenName tokenName, string tokenValue)
    {
        return await _unitOfWork.UserTokenCommandRepository.CreateUserTokenAsync(
            new UserTokenModel(
                user.UserAccountId,
                tokenName,
                // EProvider.PASSWORD,
                tokenValue
            )
        );
    }

    private async Task<UserTokenModel> UpdateUserToken(UserLoginDataModel user, ETokenName tokenName, string tokenValue)
    {
        return await _unitOfWork.UserTokenCommandRepository.UpdateUserTokenAsync(
            new UserTokenModel(
                user.UserAccountId,
                tokenName,
                // EProvider.PASSWORD,
                tokenValue
            )
        );
    }

    // private async Task SignIn(UserLoginDataModel user,  UserTokenModel token)
    // {
    //     var signInResult = await _unitOfWork.UserTokenCommandRepository.TestSignInAsync(user, token);
    //     if (!signInResult)
    //     {
    //         throw new InvalidOperationException("Failed to sign in user");
    //     }
    // }
    
    private string CreateRefreshTokenCookie(string refreshToken)
    {
        var cookieKey = _environment.GetJwtRefreshCookieKey();
        var maxAge = _environment.GetJwtRefreshTokenCookieMaxAge();
        
        return $"{cookieKey}={refreshToken}; HttpOnly; SameSite=None; Secure; Path=/; Max-Age={maxAge};";
    }
} 