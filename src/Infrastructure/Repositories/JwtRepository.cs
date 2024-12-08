// using Domain.Aggregates;
// using Domain.Interfaces;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Identity;
// using Shared.Configurations.Environment;
// using Shared.Enums;
// using Shared.Model;
//
// namespace Infrastructure.Repositories;
//
// /// <summary>
// /// Repository for handling JWT token operations, including generation, updating, and cookie management.
// /// </summary>
// public class JwtRepository : IJwtRepository
// {
//     private readonly EnvironmentConfiguration _environment;
//     private readonly IUnitOfWork _unitOfWork;
//     private readonly UserManager<UserAccountAggregate> _userManager;
//     private readonly ITokenRepository _tokenRepository;
//     private readonly IHttpContextAccessor _httpContextAccessor;
//     
//     /// <summary>
//     /// Initializes a new instance of the <see cref="JwtRepository"/> class.
//     /// </summary>
//     /// <param name="environment">Environment configuration settings.</param>
//     /// <param name="unitOfWork">Unit of work for database transactions.</param>
//     /// <param name="userManager">User manager for identity operations.</param>
//     /// <param name="tokenRepository">Token repository for token generation and retrieval.</param>
//     /// <param name="httpContextAccessor">HTTP context accessor for managing cookies.</param>
//     public JwtRepository(
//         EnvironmentConfiguration environment,
//         IUnitOfWork unitOfWork,
//         UserManager<UserAccountAggregate> userManager,
//         ITokenRepository tokenRepository,
//         IHttpContextAccessor httpContextAccessor
//         )
//     {
//         _environment = environment;
//         _unitOfWork = unitOfWork;
//         _userManager = userManager;
//         _tokenRepository = tokenRepository;
//         _httpContextAccessor = httpContextAccessor;
//     }
//
//     /// <summary>
//     /// Generates an authentication response with a refresh token cookie.
//     /// </summary>
//     /// <param name="email">The email address of the user.</param>
//     /// <returns>A <see cref="TokenModel"/> containing authentication tokens.</returns>
//     public async Task<TokenModel> GenerateAuthResponseWithRefreshTokenCookie(string email)
//     {
//         var user = await _userManager.FindByEmailAsync(email);
//         
//         var token = _tokenRepository.GenerateTokenPair(user!);
//         
//         token.LoginProvider = EProvider.PASSWORD;
//         
//         var existingTokens = await _unitOfWork.UserTokenRepository
//             .TokenExistsAsync(user!, token);
//         
//         if (!existingTokens)
//         {
//             await CreateNewTokens(user!, token);
//         }
//         else
//         {
//             var result = await _unitOfWork.UserTokenRepository.RemoveLoginAndAuthenticationTokenAsync(user!, token);
//             
//             if(!result)
//                 throw new InvalidOperationException("Failed to create user tokens");
//
//             await UpdateExistingTokens(user!, token);
//         }
//
//         CreateRefreshTokenCookie(token);
//         
//         return new TokenModel(
//             token.Token!.TokenType,
//             token.Token.AccessToken,
//             token.Token.AccessTokenExpires,
//             token.Token.RefreshToken,
//             token.Token.RefreshTokenExpires
//             );
//     }
//     
//     private async Task CreateNewTokens(UserAccountAggregate user, UserTokenModel token)
//     {
//         var accessToken = await _unitOfWork.UserTokenRepository.CreateUserTokenAsync(
//             user,
//             new UserTokenModel(
//                 user.UserProfileId,
//                 ETokenName.ACCESS,
//                 token.LoginProvider,
//                 token.Token!.AccessToken,
//                 token.Claims
//             )
//         );
//
//         var refreshToken = await _unitOfWork.UserTokenRepository.CreateUserTokenAsync(
//             user,
//             new UserTokenModel(
//                 user.UserProfileId,
//                 ETokenName.REFRESH,
//                 token.LoginProvider,
//                 token.Token.RefreshToken,
//                 token.Claims
//             )
//         );
//                 
//         if (accessToken == null || refreshToken == null)
//         {
//             throw new InvalidOperationException("Failed to create user tokens");
//         }
//     }
//
//     private async Task UpdateExistingTokens(UserAccountAggregate user, UserTokenModel token)
//     {
//         var accessToken = await _unitOfWork.UserTokenRepository.UpdateUserTokenAsync(
//             user,
//             new UserTokenModel(
//                 user.UserProfileId,
//                 ETokenName.ACCESS,
//                 token.LoginProvider,
//                 token.Token!.AccessToken,
//                 token.Claims
//             )
//         );
//
//         var refreshToken = await _unitOfWork.UserTokenRepository.UpdateUserTokenAsync(
//             user,
//             new UserTokenModel(
//                 user.UserProfileId,
//                 ETokenName.REFRESH,
//                 token.LoginProvider,
//                 token.Token.RefreshToken,
//                 token.Claims
//             )
//         );
//         
//         if (accessToken == null || refreshToken == null)
//         {
//             throw new InvalidOperationException("Failed to update user tokens");
//         }
//     }
//     
//     private void CreateRefreshTokenCookie(UserTokenModel token)
//     {
//         var cookieKey = _environment.GetJwtRefreshCookieKey();
//
//         var cookieOptions = new CookieOptions
//         {
//             HttpOnly = true,
//             Secure = true,
//             SameSite = SameSiteMode.Strict,
//             MaxAge = TimeSpan.FromHours(token.Token!.RefreshTokenExpires),
//         };
//
//         _httpContextAccessor.HttpContext?.Response.Cookies.Append(cookieKey!, token.Token.RefreshToken, cookieOptions);
//     }
// } 