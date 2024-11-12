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

    public UserTokenCommandRepository(
        ApplicationDbContext context,
        UserManager<UserLoginDataEntity> userManager,
        IMapper mapper
        )
    {
        _mapper = mapper;
        _context = context;        
        _userManager = userManager;

    }

    public async Task<UserTokenModel?> CreateUserTokenAsync(UserTokenModel request)
    {
        var userLoginDataEntity = await _userManager.Users.FirstOrDefaultAsync(u => u.UserAccountId == request.UserId);
        
        var result = await _userManager.SetAuthenticationTokenAsync(
            userLoginDataEntity,
            request.LoginProvider.ToString(),
            request.Name.ToString(),
            request.Value          
            );

        if (!result.Succeeded)
        {
            return null;
        }

        await _context.SaveChangesAsync();

        return request;
    }
    
    public async Task<bool> UpdateUserTokenAsync(UserTokenModel request)
    {
        var userLoginDataEntity = await _userManager.Users.FirstOrDefaultAsync(u => u.UserAccountId == request.UserId);
        
        var result = await _userManager.SetAuthenticationTokenAsync(
            userLoginDataEntity,
            request.LoginProvider.ToString(),
            request.Name.ToString(),
            request.Value          
        );

        if (!result.Succeeded)
        {
            return false;
        }

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<UserTokenModel>?> TokenExistsAsync(string userId, TokenModel token)
    {
        Guid.TryParse(userId, out Guid newUserAccountId);
        
        // var userToken = await _context.UserTokens
        //     .FirstOrDefaultAsync(ut => ut.UserId == newUserAccountId);
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.UserAccountId == userId);
        
        var accessToken = await _userManager.GetAuthenticationTokenAsync(
            user,
            EProvider.PASSWORD.ToString(), //ToDo inset EProvider from JwtService instead of using enum 
            ETokenName.ACCESS.ToString()
        );

        var refreshToken = await _userManager.GetAuthenticationTokenAsync(
            user,
            EProvider.PASSWORD.ToString(),
            ETokenName.REFRESH.ToString()
        );
        
        if (accessToken == null || refreshToken == null)
            return null;
        
        return _mapper.Map<List<UserTokenModel>>(token);
    }
}