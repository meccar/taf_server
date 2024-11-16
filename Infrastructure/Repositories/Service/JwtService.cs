using System.Text;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Model;
using Domain.SeedWork.Enums.Token;
using Domain.SeedWork.Enums.UserLoginDataExternal;
using Infrastructure.Configurations.Environment;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Domain.Interfaces.Service;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Repositories.Service;

public class JwtService : IJwtService
{
    private readonly EnvironmentConfiguration _environment;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<UserLoginDataEntity> _userManager;
    private readonly ITokenService _tokenService;
    private readonly byte[] _secret;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly TokenValidationParameters _tokenValidationParameters;
    
    public JwtService(
        EnvironmentConfiguration environment,
        IUnitOfWork unitOfWork,
        UserManager<UserLoginDataEntity> userManager,
        ITokenService tokenService,
        IHttpContextAccessor httpContextAccessor
        )
    {
        _environment = environment;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _tokenService = tokenService;
        _secret = Encoding.UTF8.GetBytes(environment.GetJwtSecret());
        _httpContextAccessor = httpContextAccessor;
        _tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(_secret),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    }

    public async Task<TokenModel> GenerateAuthResponseWithRefreshTokenCookie(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        
        var token = await _tokenService.GenerateTokenPair(user);
        
        token.LoginProvider = EProvider.PASSWORD;
        
        var existingTokens = await _unitOfWork.UserTokenQueryRepository
            .TokenExistsAsync(user, token);
        
        if (!existingTokens)
        {
            await CreateNewTokens(user, token);
        }
        else
        {
            var result = await _unitOfWork.UserTokenCommandRepository.RemoveLoginAndAuthenticationTokenAsync(user, token);
            
            if(!result)
                throw new InvalidOperationException("Failed to create user tokens");

            await UpdateExistingTokens(user, token);
        }

        CreateRefreshTokenCookie(token);
        
        return new TokenModel(
            token.Token.TokenType,
            token.Token.AccessToken,
            token.Token.AccessTokenExpires,
            token.Token.RefreshToken,
            token.Token.RefreshTokenExpires
            );
    }
    
    private async Task CreateNewTokens(UserLoginDataEntity user, UserTokenModel token)
    {
        var accessToken = await _unitOfWork.UserTokenCommandRepository.CreateUserTokenAsync(
            user,
            new UserTokenModel(
                user.UserAccountId,
                ETokenName.ACCESS,
                token.LoginProvider,
                token.Token.AccessToken
            )
        );

        var refreshToken = await _unitOfWork.UserTokenCommandRepository.CreateUserTokenAsync(
            user,
            new UserTokenModel(
                user.UserAccountId,
                ETokenName.REFRESH,
                token.LoginProvider,
                token.Token.RefreshToken
            )
        );
                
        if (accessToken == null || refreshToken == null)
        {
            throw new InvalidOperationException("Failed to create user tokens");
        }
    }

    private async Task UpdateExistingTokens(UserLoginDataEntity user, UserTokenModel token)
    {
        // TODO: Implement token blacklisting logic here
        
        var accessToken = await _unitOfWork.UserTokenCommandRepository.UpdateUserTokenAsync(
            user,
            new UserTokenModel(
                user.UserAccountId,
                ETokenName.ACCESS,
                token.LoginProvider,
                token.Token.AccessToken
            )
        );

        var refreshToken = await _unitOfWork.UserTokenCommandRepository.UpdateUserTokenAsync(
            user,
            new UserTokenModel(
                user.UserAccountId,
                ETokenName.REFRESH,
                token.LoginProvider,
                token.Token.RefreshToken
            )
        );
        
        if (accessToken == null || refreshToken == null)
        {
            throw new InvalidOperationException("Failed to update user tokens");
        }
    }
    
    private void CreateRefreshTokenCookie(UserTokenModel token)
    {
        var cookieKey = _environment.GetJwtRefreshCookieKey();

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            // Expires = DateTime.UtcNow.AddDays(token.Token.RefreshTokenExpires),
            MaxAge = TimeSpan.FromHours(token.Token.RefreshTokenExpires),
        };

        _httpContextAccessor.HttpContext?.Response.Cookies.Append(cookieKey, token.Token.RefreshToken, cookieOptions);
    }
} 