using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Command;
using Domain.Model;
using Domain.SeedWork.Enums.Token;
using Infrastructure.Configurations.Environment;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;


namespace Infrastructure.Repositories.Command;

public class UserTokenCommandRepository
    : IUserTokenCommandRepository
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<UserLoginDataEntity> _userManager;
    private readonly SignInManager<UserLoginDataEntity> _signInManager;
    
    public UserTokenCommandRepository(
        ApplicationDbContext context,
        UserManager<UserLoginDataEntity> userManager,
        EnvironmentConfiguration environment,
        SignInManager<UserLoginDataEntity> signInManager,
        IMapper mapper
        )
    {
        _mapper = mapper;
        _context = context;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<UserTokenModel?> CreateUserTokenAsync(UserLoginDataEntity user, UserTokenModel request)
    {
        var result = await _userManager.SetAuthenticationTokenAsync(
            user,
            request.LoginProvider.ToString(),
            request.Name.ToString(),
            request.Value          
            );

        if (result.Succeeded)
        {
            await SignInAsync(user, request);
            await _context.SaveChangesAsync();
            return request;
        }

        return null;
    }
    
    public async Task<UserTokenModel?> UpdateUserTokenAsync(UserLoginDataEntity user, UserTokenModel request)
    {
        var result = await _userManager.SetAuthenticationTokenAsync(
            user,
            request.LoginProvider.ToString(),
            request.Name.ToString(),
            request.Value          
        );

        if (result.Succeeded)
        {
            await UpdateSignInAsync(user, request);
            await _context.SaveChangesAsync();
            return request;
        }

        return null;
    }

    public async Task<bool> RemoveLoginAndAuthenticationTokenAsync(UserLoginDataEntity userLoginDataEntity, UserTokenModel token)
    {
        foreach (var claim in token.Claims)
        {
            await  _userManager.RemoveClaimAsync(userLoginDataEntity, claim);;
        }
        
        var removeLogin = await _userManager
            .RemoveLoginAsync(
                userLoginDataEntity,
                token.LoginProvider.ToString(),
                userLoginDataEntity.Email);
        
        var removeAuthenticationTokenResult = await _userManager
            .RemoveAuthenticationTokenAsync(
                userLoginDataEntity, 
                token.LoginProvider.ToString(), 
                token.Name.ToString());

        await _signInManager.SignOutAsync();
        
        if(removeLogin.Succeeded && removeAuthenticationTokenResult.Succeeded)
            return true;

        return false;
    }

    private async Task<SignInResult> SignInAsync(UserLoginDataEntity userLoginDataModel, UserTokenModel token)
    {
        var loginInfo = new UserLoginInfo(
            token.LoginProvider.ToString(),
            userLoginDataModel.Email,
            token.LoginProvider.ToString()
        );
        
        var loginResult = await _userManager.AddLoginAsync(userLoginDataModel, loginInfo);
        if (!loginResult.Succeeded) return SignInResult.Failed;
        
        await _signInManager.SignInAsync(userLoginDataModel, isPersistent: false);
        
        foreach (var claim in token.Claims)
        {
            await _userManager.AddClaimAsync(userLoginDataModel, claim);
        }
        
        return await _signInManager.PasswordSignInAsync(
            userLoginDataModel,
            userLoginDataModel.PasswordHash,
            isPersistent: false,
            lockoutOnFailure: false
        );;
    }
    
    private async Task<SignInResult> UpdateSignInAsync(UserLoginDataEntity userLoginDataModel, UserTokenModel token)
    {
        var loginInfo = new UserLoginInfo(
            token.LoginProvider.ToString(),
            userLoginDataModel.Email,
            token.LoginProvider.ToString()
        );
        
        var loginResult = await _userManager.AddLoginAsync(userLoginDataModel, loginInfo);
        if (!loginResult.Succeeded) return SignInResult.Failed;
        
        await _signInManager.RefreshSignInAsync(userLoginDataModel);
        
        foreach (var claim in token.Claims)
        {
            await _userManager.AddClaimAsync(userLoginDataModel, claim);
        }
        
        return await _signInManager.PasswordSignInAsync(
            userLoginDataModel,
            userLoginDataModel.PasswordHash,
            isPersistent: false,
            lockoutOnFailure: false
        );;
    }
}