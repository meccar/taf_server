using Domain.Aggregates;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Shared.Configurations.Environment;
using Shared.Enums;
using Shared.Model;
using Shared.Results;

namespace Persistance.Repositories;

/// <summary>
/// Repository for handling JWT token operations, including generation, updating, and cookie management.
/// </summary>
public class JwtRepository : IJwtRepository
{
    private readonly EnvironmentConfiguration _environment;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<UserAccountAggregate> _userManager;
    private readonly ITokenRepository _tokenRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="JwtRepository"/> class.
    /// </summary>
    /// <param name="environment">Environment configuration settings.</param>
    /// <param name="unitOfWork">Unit of work for database transactions.</param>
    /// <param name="userManager">User manager for identity operations.</param>
    /// <param name="tokenRepository">Token repository for token generation and retrieval.</param>
    /// <param name="httpContextAccessor">HTTP context accessor for managing cookies.</param>
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
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Generates an authentication response with a refresh token cookie.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <returns>A <see cref="TokenModel"/> containing authentication tokens.</returns>
    public async Task<Result<TokenModel>> GenerateAuthResponseWithRefreshTokenCookie(
        UserAccountAggregate user,
        UserTokenModel token
    )
    {
        var refreshToken = await _unitOfWork
            .UserTokenRepository
            .CreateUserTokenAsync(
            user,
            new UserTokenModel(
                user.UserProfileId,
                token.Name,
                token.LoginProvider,
                token.Token!.RefreshToken,
                token.Claims
            )
        );

        if (refreshToken == null)
            return Result<TokenModel>.Failure("Failed while logging in, please try again.");
        
        return Result<TokenModel>
            .Success(new TokenModel(
            token.Token!.TokenType,
            token.Token.AccessToken,
            token.Token.AccessTokenExpires,
            token.Token.RefreshToken,
            token.Token.RefreshTokenExpires
        ));
    }
    
    public async Task<TokenModel> GenerateAuthResponseWithRefreshTokenCookie(
        UserAccountAggregate user
    )
    {
        var token = _tokenRepository
            .GenerateTokenPair(user);
        
        // token.LoginProvider = EProvider.PASSWORD;
        // token.Name = ETokenName.REFRESH;
        
        var result = await _unitOfWork
            .UserTokenRepository
            .RemoveLoginAndAuthenticationTokenAsync(user!, token);
        
        if(!result)
            throw new InvalidOperationException("Failed to create user tokens");
        
        var refreshToken = await _unitOfWork
            .UserTokenRepository
            .CreateUserTokenAsync(
                user,
                new UserTokenModel(
                    user.UserProfileId,
                    token.Name,
                    token.LoginProvider,
                    token.Token!.RefreshToken,
                    token.Claims
                )
            );
                
        if (refreshToken == null)
            throw new InvalidOperationException("Failed to create user tokens");
        
        // CreateRefreshTokenCookie(token);
        return new TokenModel(
            token.Token!.TokenType,
            token.Token.AccessToken,
            token.Token.AccessTokenExpires,
            token.Token.RefreshToken,
            token.Token.RefreshTokenExpires
        );
    }
    
    private void CreateRefreshTokenCookie(UserTokenModel token)
    {
        var cookieKey = _environment.GetJwtRefreshCookieKey();

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            MaxAge = TimeSpan.FromHours(token.Token!.RefreshTokenExpires),
        };

        _httpContextAccessor.HttpContext?.Response.Cookies.Append(cookieKey!, token.Token.RefreshToken, cookieOptions);
    }
} 