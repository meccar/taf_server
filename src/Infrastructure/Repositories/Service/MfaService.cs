using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Service;
using Microsoft.AspNetCore.Identity;
using Shared.Model;

namespace Infrastructure.Repositories.Service;

public class MfaService : IMfaService 
{
    private readonly UserManager<UserAccountAggregate> _userManager;

    public MfaService(
        UserManager<UserAccountAggregate> userManager
    )
    {
        _userManager = userManager;
    }

    public async Task<bool> MfaSetup(UserAccountAggregate user)
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
    
    public async Task<bool> MfaSetup(MfaViewModel model,UserAccountAggregate user)
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