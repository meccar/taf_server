using Domain.Aggregates;
using Domain.Interfaces.Credentials;
using Microsoft.AspNetCore.Identity;
using Shared.Model;

namespace Persistance.Repositories.Credentials;

/// <summary>
/// Repository for managing user tokens.
/// </summary>
public class UserTokenRepository
    : IUserTokenRepository
{
    private readonly UserManager<UserAccountAggregate> _userManager;
    private readonly SignInManager<UserAccountAggregate> _signInManager;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="UserTokenRepository"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="userManager">The user manager for user accounts.</param>
    /// <param name="signInManager">The sign-in manager for user accounts.</param>
    public UserTokenRepository(
        UserManager<UserAccountAggregate> userManager,
        SignInManager<UserAccountAggregate> signInManager
    )
    {
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
            return request;

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
}