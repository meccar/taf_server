using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Service;
using Domain.Model;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories.Service;

public class MfaService : IMfaService 
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<UserLoginDataEntity> _userManager;

    public MfaService(
        IUnitOfWork unitOfWork,
        UserManager<UserLoginDataEntity> userManager
    )
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<bool> MfaSetup(UserLoginDataEntity user)
    {
        var token = await _userManager.GetAuthenticatorKeyAsync(user);
        await _userManager.ResetAuthenticatorKeyAsync(user);
        var model = new MfaViewModel { Token = token };
        return true;
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