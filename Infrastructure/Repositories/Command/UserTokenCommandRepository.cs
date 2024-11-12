using AutoMapper;
using Azure.Core;
using Domain.Entities;
using Domain.Interfaces.Command;
using Domain.Model;
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
            request.LoginProvider,
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
        var userTokenEntity = _mapper.Map<IdentityUserToken<Guid>>(request);

        _context.UserTokens.Update(userTokenEntity);

        var result = await _context.SaveChangesAsync();

        return result > 0;
    }

    public async Task<List<UserTokenModel>?> GetUserTokensByUserAccountId(string userAccountId)
    {
        Guid.TryParse(userAccountId, out Guid newUserAccountId);
        
        var userToken = await _context.UserTokens
            .Where(ut => ut.UserId == newUserAccountId)
            .FirstOrDefaultAsync();

        if (userToken == null)
            return null;
        
        return _mapper.Map<List<UserTokenModel>>(userToken);
    }
}