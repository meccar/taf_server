using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Command;
using Domain.Interfaces.Query;
using Domain.Model;
using Domain.SeedWork.Enums.Token;
using Infrastructure.Configurations.Environment;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories.Query;

public class UserTokenQueryRepository
    : IUserTokenQueryRepository
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<UserLoginDataEntity> _userManager;
    private readonly SignInManager<UserLoginDataEntity> _signInManager;
    
    public UserTokenQueryRepository(
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

    public async Task<bool> TokenExistsAsync(UserLoginDataEntity user, UserTokenModel token)
    {
        // var existingLogins = await _userManager.GetLoginsAsync(user);
        // var existingLogin = existingLogins
        //     .FirstOrDefault(l => l.LoginProvider == token.LoginProvider.ToString());
        //
        // if (existingLogin == null) return false;
        
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
            return false;

        return true;
    }
}