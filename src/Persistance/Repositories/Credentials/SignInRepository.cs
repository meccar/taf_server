using Domain.Aggregates;
using Domain.Interfaces;
using Domain.Interfaces.Credentials;
using Domain.Interfaces.Tokens;
using Microsoft.AspNetCore.Identity;
using Shared.Enums;
using Shared.Model;
using Shared.Results;

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

    public async Task<Result<(UserAccountAggregate, UserTokenModel)>> SignInAsync(
        string email,
        string? password,
        bool isPersistent
    )
    {
        UserAccountAggregate? user = await _userManager.FindByEmailAsync(email);

        if (user == null)
            return Result<(UserAccountAggregate, UserTokenModel)>.Failure("Invalid email or Password");
        
        var token = _tokenRepository.GenerateTokenPair(user);
        
        // password = "Password@1234";
        
        // If no password is provided, attempt external sign-in
        if(password == null)
            return await ExternalSignInAsync(user, token);
        
        // var claimsIdentity = new ClaimsIdentity(token.Claims, user.EId);
        // var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        //
        // if (_signInManager.IsSignedIn(claimsPrincipal))
        //     return Result<(UserAccountAggregate, UserTokenModel)>.Failure("User has already signed in");

        return await InternalSignInAsync(user, password, isPersistent, token);
    }

    private async Task<Result<(UserAccountAggregate, UserTokenModel)>> ExternalSignInAsync(
        UserAccountAggregate userAccountAggregate,
        UserTokenModel userTokenModel
    )
    {
        if (!userAccountAggregate.IsTwoFactorVerified)
            return Result<(UserAccountAggregate, UserTokenModel)>.Failure("Account has not been verified");
        
        var signInResult = await _userManager.AddLoginAsync(userAccountAggregate, new UserLoginInfo(            
            userTokenModel.LoginProvider.ToString()!,
            userAccountAggregate.Email!,
            userTokenModel.LoginProvider.ToString()));
        
        return signInResult.Succeeded 
            ? Result<(UserAccountAggregate, UserTokenModel)>.Success((userAccountAggregate, userTokenModel))
            : Result<(UserAccountAggregate, UserTokenModel)>.Failure("Invalid email or Password");
    }
    
    private async Task<Result<(UserAccountAggregate, UserTokenModel)>> InternalSignInAsync(
        UserAccountAggregate userAccountAggregate,
        string password, 
        bool isPersistent, 
        UserTokenModel token
    )
    {
        if (!await _userManager.CheckPasswordAsync(userAccountAggregate, password))
            return Result<(UserAccountAggregate, UserTokenModel)>.Failure("Invalid email or Password");
        
        if (!userAccountAggregate.IsTwoFactorVerified)
            return Result<(UserAccountAggregate, UserTokenModel)>.Failure("Account has not been verified");
        
        await _signInManager.RememberTwoFactorClientAsync(userAccountAggregate);
        
        var signInResult = await _signInManager.PasswordSignInAsync(
            userAccountAggregate.UserName!,
            password,
            isPersistent,
            true
        );

        password = null!;
        
        if (!signInResult.Succeeded)
            return Result<(UserAccountAggregate, UserTokenModel)>.Failure("Invalid email or Password");

        token.LoginProvider = EProvider.PASSWORD;
        token.Name = ETokenName.REFRESH;
        
        var removeLoginAndAuthenticationTokenResult = await _unitOfWork
            .UserTokenRepository
            .RemoveLoginAndAuthenticationTokenAsync(userAccountAggregate, token);
        
        if(!removeLoginAndAuthenticationTokenResult)
            return Result<(UserAccountAggregate, UserTokenModel)>.Failure("Invalid email or Password");
        
        foreach (var claim in token.Claims)
            await _userManager.AddClaimAsync(userAccountAggregate, claim);
        
        await _signInManager.SignInWithClaimsAsync(userAccountAggregate, isPersistent, token.Claims);
        return Result<(UserAccountAggregate, UserTokenModel)>.Success((userAccountAggregate, token));
    }
}