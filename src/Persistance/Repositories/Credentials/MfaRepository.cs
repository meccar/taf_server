using System.Net;
using Domain.Aggregates;
using Domain.Interfaces.Credentials;
using Microsoft.AspNetCore.Identity;
using Shared.Model;

namespace Persistance.Repositories.Credentials;

/// <summary>
/// Repository for handling Multi-Factor Authentication (MFA) operations.
/// </summary>
public class MfaRepository : IMfaRepository 
{
    private readonly UserManager<UserAccountAggregate> _userManager;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="MfaRepository"/> class.
    /// </summary>
    /// <param name="userManager">The UserManager instance used for managing user accounts.</param>
    public MfaRepository(
        UserManager<UserAccountAggregate> userManager
    )
    {
        _userManager = userManager;
    }

    /// <summary>
    /// Sets up MFA for a user by generating an authenticator key and a QR code URI.
    /// </summary>
    /// <param name="user">The user account for which MFA is being set up.</param>
    /// <returns>A <see cref="MfaViewModel"/> containing the shared key and QR code URI.</returns>
    public async Task<MfaViewModel?> MfaSetup(UserAccountAggregate user)
    {
        // Disable 2FA first
        await _userManager.SetTwoFactorEnabledAsync(user, false);
    
        // Generate a new key
        await _userManager.ResetAuthenticatorKeyAsync(user);
        string? unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
    
        if (string.IsNullOrEmpty(unformattedKey))
            return null;

        await _userManager.UpdateSecurityStampAsync(user);
        
        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
            return null;
        
        // Enable 2FA
        var result = await _userManager.SetTwoFactorEnabledAsync(user, true);
        if (!result.Succeeded)
            return null;

        return new MfaViewModel
        {
            SharedKey = unformattedKey,
            AuthenticatorUri = GenerateQrCodeUri(user.Email!, unformattedKey)
        };
    }

    /// <summary>
    /// Validates the MFA token for the specified user.
    /// </summary>
    /// <param name="user">The user attempting to validate the MFA token.</param>
    /// <param name="token">The MFA token provided by the user.</param>
    /// <returns>A <see cref="Result"/> indicating success or failure.</returns>
    public async Task<bool> ValidateMfa(UserAccountAggregate user, string token)
    {
        if (!await _userManager.GetTwoFactorEnabledAsync(user))
            return false;
        
        var providers = await _userManager.GetValidTwoFactorProvidersAsync(user); 
        if (!providers.Contains(_userManager.Options.Tokens.AuthenticatorTokenProvider))
            return false;

        // using _userManager.GenerateTwoFactorTokenAsync because TwoFactorToken is actually
        // the token sent through SMS
        // TODO: setup for Authenticator apps
        
        var test = await _userManager.GenerateTwoFactorTokenAsync(
            user,
            _userManager.Options.Tokens.ChangePhoneNumberTokenProvider
        );
        
        bool isValidToken = await _userManager.VerifyTwoFactorTokenAsync(
            user,
            _userManager.Options.Tokens.ChangePhoneNumberTokenProvider,
            test
        );

        if (isValidToken)
        {
            user.TwoFactorSecret = token;
            user.IsTwoFactorVerified = true;

            var updateResult = await _userManager.UpdateAsync(user);
            
            return updateResult.Succeeded;
        }
        
        return false;
    }
    
    /// <summary>
    /// Generates a QR code URI for MFA setup.
    /// </summary>
    /// <param name="email">The email of the user for whom the QR code URI is generated.</param>
    /// <param name="unformattedKey">The unformatted key used for MFA generation.</param>
    /// <returns>A URI string for the QR code used by the authenticator app.</returns>
    private string GenerateQrCodeUri(string email, string unformattedKey)
    {
        const string authenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
 
        return string.Format(
            authenticatorUriFormat,
            WebUtility.UrlEncode("TAF Việt"),
            WebUtility.UrlEncode(email),
            unformattedKey);
    }
}