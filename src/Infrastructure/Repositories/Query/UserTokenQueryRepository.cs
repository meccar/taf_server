using AutoMapper;
using DataBase.Data;
using Domain.Entities;
using Domain.Interfaces.Query;
using Microsoft.AspNetCore.Identity;
using Share.Configurations.Environment;
using Shared.Enums;
using Shared.Model;

namespace Infrastructure.Repositories.Query;

public class UserTokenQueryRepository
    : IUserTokenQueryRepository
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<UserAccountAggregate> _userManager;
    private readonly SignInManager<UserAccountAggregate> _signInManager;
    
    public UserTokenQueryRepository(
        ApplicationDbContext context,
        UserManager<UserAccountAggregate> userManager,
        EnvironmentConfiguration environment,
        SignInManager<UserAccountAggregate> signInManager,
        IMapper mapper
        )
    {
        _mapper = mapper;
        _context = context;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<bool> TokenExistsAsync(UserAccountAggregate user, UserTokenModel token)
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