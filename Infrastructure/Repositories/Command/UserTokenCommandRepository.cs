using AutoMapper;
using Azure.Core;
using Domain.Entities;
using Domain.Interfaces.Command;
using Domain.Model;
using Domain.SeedWork.Enums.Token;
using Domain.SeedWork.Enums.UserLoginDataExternal;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
        SignInManager<UserLoginDataEntity> signInManager,
        IMapper mapper
        )
    {
        _mapper = mapper;
        _context = context;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<UserTokenModel?> CreateUserTokenAsync(UserTokenModel request)
    {
        var user = await GetUserById(request.UserId);
        
        var result = await _userManager.SetAuthenticationTokenAsync(
            user,
            request.LoginProvider.ToString(),
            request.Name.ToString(),
            request.Value          
            );

        if (result.Succeeded)
        {
            await TestSignInAsync(user, request);
            await _context.SaveChangesAsync();
            return request;
        }

        return null;
    }
    
    public async Task<UserTokenModel?> UpdateUserTokenAsync(UserTokenModel request)
    {
        var user = await GetUserById(request.UserId);
        
        var result = await _userManager.SetAuthenticationTokenAsync(
            user,
            request.LoginProvider.ToString(),
            request.Name.ToString(),
            request.Value          
        );

        if (result.Succeeded)
        {
            await TestSignInAsync(user, request);
            await _context.SaveChangesAsync();
            return request;
        }

        return null;
    }

    public async Task<List<UserTokenModel>?> TokenExistsAsync(string userId, UserTokenModel token)
    {
        Guid.TryParse(userId, out Guid newUserAccountId);
        
        var user = await GetUserByGuid(userId);
        
        var accessToken = await _userManager.GetAuthenticationTokenAsync(
            user,
            token.LoginProvider.ToString(),
            ETokenName.ACCESS.ToString()
        );

        var refreshToken = await _userManager.GetAuthenticationTokenAsync(
            user,
            token.LoginProvider.ToString(),
            ETokenName.REFRESH.ToString()
        );
        
        if (accessToken == null || refreshToken == null)
            return null;
        
        return _mapper.Map<List<UserTokenModel>>(token);
    }

    public async Task<bool> TestSignInAsync(UserLoginDataEntity userLoginDataModel, UserTokenModel token)
    {
        // var user = await GetUserByGuid(userLoginDataModel.Id);
        
        var loginInfo = new UserLoginInfo(
            token.LoginProvider.ToString(),
            token.Token.AccessToken,
            token.Name?.ToString() ?? string.Empty
        );
        
        await _userManager.RemoveLoginAsync(userLoginDataModel, token.LoginProvider.ToString(), token.Token.AccessToken);
        var loginResult = await _userManager.AddLoginAsync(userLoginDataModel, loginInfo);
        if (!loginResult.Succeeded) return false;
        
        await _signInManager.SignInAsync(userLoginDataModel, isPersistent: false);
        return true;
    }
    
    private async Task<UserLoginDataEntity?> GetUserById(string userAccountId)
    {
        return await _userManager.Users
            .FirstOrDefaultAsync(u => u.UserAccountId == userAccountId);
    }

    private async Task<UserLoginDataEntity?> GetUserByGuid(string userId)
    {
        if (!Guid.TryParse(userId, out Guid userGuid))
            return null;

        return await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == userGuid);
    }
    
    private async Task<bool> SetAuthenticationToken(UserLoginDataEntity user, UserTokenModel token)
    {
        var result = await _userManager.SetAuthenticationTokenAsync(
            user,
            token.LoginProvider.ToString(),
            token.Name.ToString(),
            token.Value
        );

        if (result.Succeeded)
        {
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }
    
    private async Task<(bool, bool)> GetUserTokens(UserLoginDataEntity user, UserTokenModel token)
    {
        var accessToken = await _userManager.GetAuthenticationTokenAsync(
            user,
            token.LoginProvider.ToString(),
            ETokenName.ACCESS.ToString()
        );

        var refreshToken = await _userManager.GetAuthenticationTokenAsync(
            user,
            token.LoginProvider.ToString(),
            ETokenName.REFRESH.ToString()
        );

        return (accessToken != null, refreshToken != null);
    }
}