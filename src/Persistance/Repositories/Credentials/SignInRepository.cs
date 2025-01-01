using Domain.Aggregates;
using Domain.Interfaces;
using Domain.Interfaces.Credentials;
using Domain.Interfaces.Tokens;
using Microsoft.AspNetCore.Identity;
using Shared.Enums;
using Shared.Model;

namespace Persistance.Repositories.Credentials;

public class SignInRepository
    : ISignInRepository
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<UserAccountAggregate> _userManager;
    private readonly SignInManager<UserAccountAggregate> _signInManager;
    private readonly ITokenRepository _tokenRepository;

    public SignInRepository(
        IUnitOfWork unitOfWork,
        UserManager<UserAccountAggregate> userManager,
        SignInManager<UserAccountAggregate> signInManager,
        ITokenRepository tokenRepository
    )
    {
        _unitOfWork = unitOfWork;
        _signInManager = signInManager;
        _userManager = userManager;
        _tokenRepository = tokenRepository;
    }

    public async Task<(UserAccountAggregate, UserTokenModel)?> SignInAsync(
        UserAccountAggregate user,
        string? password,
        bool isPersistent
    )
    {
        var token = _tokenRepository.GenerateTokenPair(user);
        
        // password = "Password@1234";
        
        // If no password is provided, attempt external sign-in
        if(password == null)
            return await ExternalSignInAsync(user, token);

        return await InternalSignInAsync(user, password, isPersistent, token);
    }

    private async Task<(UserAccountAggregate, UserTokenModel)?> ExternalSignInAsync(
        UserAccountAggregate userAccountAggregate,
        UserTokenModel userTokenModel
    )
    {
        if (!userAccountAggregate.IsTwoFactorVerified)
            return null;
        
        var signInResult = await _userManager.AddLoginAsync(userAccountAggregate, new UserLoginInfo(            
            userTokenModel.LoginProvider.ToString()!,
            userAccountAggregate.Email!,
            userTokenModel.LoginProvider.ToString()));
        
        return signInResult.Succeeded 
            ? (userAccountAggregate, userTokenModel)
            : null;
    }
    
    private async Task<(UserAccountAggregate, UserTokenModel)?> InternalSignInAsync(
        UserAccountAggregate userAccountAggregate,
        string password, 
        bool isPersistent, 
        UserTokenModel token
    )
    {
        if (!await _userManager.CheckPasswordAsync(userAccountAggregate, password))
            return null;
        
        if (!userAccountAggregate.IsTwoFactorVerified)
            return null;
        
        await _signInManager.RememberTwoFactorClientAsync(userAccountAggregate);
        
        var signInResult = await _signInManager.PasswordSignInAsync(
            userAccountAggregate.UserName!,
            password,
            isPersistent,
            true
        );

        password = null!;
        
        if (!signInResult.Succeeded)
            return null;

        token.LoginProvider = EProvider.PASSWORD;
        token.Name = ETokenName.REFRESH;
        
        var removeLoginAndAuthenticationTokenResult = await _unitOfWork
            .UserTokenRepository
            .RemoveLoginAndAuthenticationTokenAsync(userAccountAggregate, token);
        
        if(!removeLoginAndAuthenticationTokenResult)
            return null;
        
        foreach (var claim in token.Claims)
            await _userManager.AddClaimAsync(userAccountAggregate, claim);
        
        await _signInManager.SignInWithClaimsAsync(userAccountAggregate, isPersistent, token.Claims);
        return (userAccountAggregate, token);
    }
}