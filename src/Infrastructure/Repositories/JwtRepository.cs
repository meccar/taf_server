using System.Text;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Shared.Configurations.Environment;
using Shared.Enums;
using Shared.Model;

namespace Infrastructure.Repositories;

public class JwtRepository : IJwtRepository
{
    private readonly EnvironmentConfiguration _environment;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<UserAccountAggregate> _userManager;
    private readonly ITokenRepository _tokenRepository;
    private readonly byte[] _secret;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly TokenValidationParameters _tokenValidationParameters;
    
    public JwtRepository(
        EnvironmentConfiguration environment,
        IUnitOfWork unitOfWork,
        UserManager<UserAccountAggregate> userManager,
        ITokenRepository tokenRepository,
        IHttpContextAccessor httpContextAccessor
        )
    {
        _environment = environment;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _tokenRepository = tokenRepository;
        _secret = Encoding.UTF8.GetBytes(environment.GetJwtSecret());
        _httpContextAccessor = httpContextAccessor;
        _tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(_secret),
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    }

    public async Task<TokenModel> GenerateAuthResponseWithRefreshTokenCookie(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        
        var token = await _tokenRepository.GenerateTokenPair(user);

        // var accessToken = await _httpContextAccessor
        //     .HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
        //
        // var refreshToken = await _httpContextAccessor
        //     .HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
        
        token.LoginProvider = EProvider.PASSWORD;
        
        var existingTokens = await _unitOfWork.UserTokenRepository
            .TokenExistsAsync(user, token);
        
        if (!existingTokens)
        {
            await CreateNewTokens(user, token);
        }
        else
        {
            var result = await _unitOfWork.UserTokenRepository.RemoveLoginAndAuthenticationTokenAsync(user, token);
            
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
    
    private async Task CreateNewTokens(UserAccountAggregate user, UserTokenModel token)
    {
        var accessToken = await _unitOfWork.UserTokenRepository.CreateUserTokenAsync(
            user,
            new UserTokenModel(
                user.UserProfileId,
                ETokenName.ACCESS,
                token.LoginProvider,
                token.Token.AccessToken,
                token.Claims
            )
        );

        var refreshToken = await _unitOfWork.UserTokenRepository.CreateUserTokenAsync(
            user,
            new UserTokenModel(
                user.UserProfileId,
                ETokenName.REFRESH,
                token.LoginProvider,
                token.Token.RefreshToken,
                token.Claims
            )
        );
                
        if (accessToken == null || refreshToken == null)
        {
            throw new InvalidOperationException("Failed to create user tokens");
        }
    }

    private async Task UpdateExistingTokens(UserAccountAggregate user, UserTokenModel token)
    {
        var accessToken = await _unitOfWork.UserTokenRepository.UpdateUserTokenAsync(
            user,
            new UserTokenModel(
                user.UserProfileId,
                ETokenName.ACCESS,
                token.LoginProvider,
                token.Token.AccessToken,
                token.Claims
            )
        );

        var refreshToken = await _unitOfWork.UserTokenRepository.UpdateUserTokenAsync(
            user,
            new UserTokenModel(
                user.UserProfileId,
                ETokenName.REFRESH,
                token.LoginProvider,
                token.Token.RefreshToken,
                token.Claims
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