using System.Net;
using Domain.Aggregates;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Shared.Model;
using Shared.Results;

namespace Infrastructure.Repositories;

public class MfaRepository : IMfaRepository 
{
    private readonly UserManager<UserAccountAggregate> _userManager;
    public MfaRepository(
        UserManager<UserAccountAggregate> userManager
    )
    {
        _userManager = userManager;
    }

    public async Task<MfaViewModel> MfaSetup(UserAccountAggregate user)
    {
        string? unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrEmpty(unformattedKey))
        {
            await _userManager.ResetAuthenticatorKeyAsync(user);
            unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
        }
        
        return new MfaViewModel
        {
            SharedKey = FormatKey(unformattedKey),
            AuthenticatorUri = GenerateQrCodeUri(user.Email!, unformattedKey)
        };
    }

    public async Task<Result> ValidateMfa(string email, string token)
    {
        UserAccountAggregate? user = await _userManager.FindByEmailAsync(email);
        
        if (user == null)
            return Result.Failure("Account does not exist");
        
        bool isValidToken = await _userManager.VerifyTwoFactorTokenAsync(user, "Authenticator", token);

        if (isValidToken)
        {

            return Result.Success();
        }
        
        return Result.Failure("Invalid authenticator key.");
    }
    
    private string GenerateQrCodeUri(string email, string unformattedKey)
    {
        const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
 
        return string.Format(
            AuthenticatorUriFormat,
            WebUtility.UrlEncode("TAF Việt"),
            WebUtility.UrlEncode(email),
            unformattedKey);
    }
    private string FormatKey(string unformattedKey)
    {
        return unformattedKey;
    }
}