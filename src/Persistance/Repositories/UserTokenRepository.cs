using Domain.Aggregates;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Persistance.Data;
using Shared.Enums;
using Shared.Model;

namespace Persistance.Repositories;

/// <summary>
/// Repository for managing user tokens.
/// </summary>
public class UserTokenRepository
    : IUserTokenRepository
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<UserAccountAggregate> _userManager;
    private readonly SignInManager<UserAccountAggregate> _signInManager;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="UserTokenRepository"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="userManager">The user manager for user accounts.</param>
    /// <param name="signInManager">The sign-in manager for user accounts.</param>
    public UserTokenRepository(
        ApplicationDbContext context,
        UserManager<UserAccountAggregate> userManager,
        SignInManager<UserAccountAggregate> signInManager
    )
    {
        _context = context;
        _signInManager = signInManager;
        _userManager = userManager;
    }
    
    /// <inheritdoc/>
    public async Task<UserTokenModel?> CreateUserTokenAsync(UserAccountAggregate user, UserTokenModel request)
    {
        var result = await _userManager.SetAuthenticationTokenAsync(
            user,
            request.LoginProvider.ToString()!,
            request.Name.ToString()!,
            request.Value          
        );

        if (result.Succeeded)
        {
            var signInResult = await SignInAsync(user, request);
            
            if(!signInResult.Succeeded)
                return null;
            
            await _context.SaveChangesAsync();
            return request;
        }

        return null;
    }
    
    /// <inheritdoc/>
    public async Task<UserTokenModel?> UpdateUserTokenAsync(UserAccountAggregate user, UserTokenModel request)
    {
        var result = await _userManager.SetAuthenticationTokenAsync(
            user,
            request.LoginProvider.ToString()!,
            request.Name.ToString()!,
            request.Value          
        );

        if (result.Succeeded)
        {
            var updateSignInResult = await UpdateSignInAsync(user, request);
            
            if(!updateSignInResult.Succeeded)
                return null;
            
            await _context.SaveChangesAsync();
            return request;
        }

        return null;
    }
    
    /// <inheritdoc/>
    public async Task<bool> RemoveLoginAndAuthenticationTokenAsync(UserAccountAggregate userAccountAggregate, UserTokenModel token)
    {
        foreach (var claim in token.Claims)
        {
            await  _userManager.RemoveClaimAsync(userAccountAggregate, claim);
        }
        
        var removeLogin = await _userManager
            .RemoveLoginAsync(
                userAccountAggregate,
                token.LoginProvider.ToString()!,
                userAccountAggregate.Email!);
        
        var removeAuthenticationTokenResult = await _userManager
            .RemoveAuthenticationTokenAsync(
                userAccountAggregate, 
                token.LoginProvider.ToString()!, 
                token.Name.ToString()!);

        await _signInManager.SignOutAsync();
        
        if(removeLogin.Succeeded && removeAuthenticationTokenResult.Succeeded)
            return true;

        return false;
    }
    
    /// <inheritdoc/>
    public async Task<bool> TokenExistsAsync(UserAccountAggregate user, UserTokenModel token)
    {
        var accessToken = await _userManager.GetAuthenticationTokenAsync(
            user,
            token.LoginProvider.ToString()!,
            ETokenName.ACCESS.ToString()
        );

        var refreshToken = await _userManager.GetAuthenticationTokenAsync(
            user,
            token.LoginProvider.ToString()!,
            ETokenName.REFRESH.ToString()
        );
        
        if (accessToken == null || refreshToken == null)
            return false;

        return true;
    }
    private async Task<SignInResult> SignInAsync(UserAccountAggregate userLoginDataModel, UserTokenModel token)
    {
        var loginInfo = new UserLoginInfo(
            token.LoginProvider.ToString()!,
            userLoginDataModel.Email!,
            token.LoginProvider.ToString()
        );
        
        var loginResult = await _userManager.AddLoginAsync(userLoginDataModel, loginInfo);
        if (!loginResult.Succeeded) return SignInResult.Failed;
        
        var authenticationProperties = new AuthenticationProperties
        {
            IsPersistent = false,
            AllowRefresh = true,
            RedirectUri = "/"
        };
        
        await _signInManager.SignInWithClaimsAsync(userLoginDataModel, authenticationProperties, token.Claims);
        
        foreach (var claim in token.Claims)
        {
            await _userManager.AddClaimAsync(userLoginDataModel, claim);
        }
        
        return await _signInManager.PasswordSignInAsync(
            userLoginDataModel,
            userLoginDataModel.PasswordHash!,
            isPersistent: false,
            lockoutOnFailure: false
        );
    }
    
    private async Task<SignInResult> UpdateSignInAsync(UserAccountAggregate userLoginDataModel, UserTokenModel token)
    {
        var loginInfo = new UserLoginInfo(
            token.LoginProvider.ToString()!,
            userLoginDataModel.Email!,
            token.LoginProvider.ToString()
        );
        
        var loginResult = await _userManager.AddLoginAsync(userLoginDataModel, loginInfo);
        if (!loginResult.Succeeded) return SignInResult.Failed;
        
        var authenticationProperties = new AuthenticationProperties
        {
            IsPersistent = false,
            AllowRefresh = true,
            RedirectUri = "/"
        };
        
        await _signInManager.SignInWithClaimsAsync(userLoginDataModel, authenticationProperties, token.Claims);

        foreach (var claim in token.Claims)
        {
            await _userManager.AddClaimAsync(userLoginDataModel, claim);
        }
        
        return await _signInManager.PasswordSignInAsync(
            userLoginDataModel,
            userLoginDataModel.PasswordHash!,
            isPersistent: false,
            lockoutOnFailure: false
        );
    }
}