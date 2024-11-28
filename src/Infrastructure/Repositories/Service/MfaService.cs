using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Service;
using Domain.Model;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories.Service;

public class MfaService : IMfaService 
{
    private readonly UserManager<UserLoginDataEntity> _userManager;

    public MfaService(
        UserManager<UserLoginDataEntity> userManager
    )
    {
        _userManager = userManager;
    }

    public async Task<bool> MfaSetup(UserLoginDataEntity user)
    {
        var token = await _userManager.GetAuthenticatorKeyAsync(user);
        
        if (string.IsNullOrEmpty(token))
        {
            return false;
        }
        
        var result = await _userManager.ResetAuthenticatorKeyAsync(user);

        if (result.Succeeded)
        {
            var model = new MfaViewModel { Token = token };
            return true;
        }
        return false;
    }
    
    public async Task<bool> MfaSetup(MfaViewModel model,UserLoginDataEntity user)
    {

        var result = await _userManager.VerifyUserTokenAsync(
            user, 
            _userManager.Options.Tokens.AuthenticatorTokenProvider,
            model.Code,
            model.Token
        );
        if (result)
        {
            await _userManager.SetTwoFactorEnabledAsync(user, true);
            return true;
        }

        return false;
    }
}