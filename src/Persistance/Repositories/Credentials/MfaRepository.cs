using System.Net;
using Domain.Aggregates;
using Domain.Interfaces;
using Domain.Interfaces.Credentials;
using Microsoft.AspNetCore.Identity;
using Shared.Model;
using Shared.Results;

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
    public async Task<Result<MfaViewModel>> MfaSetup(UserAccountAggregate user)
    {
        // Disable 2FA first
        await _userManager.SetTwoFactorEnabledAsync(user, false);
    
        // Generate a new key
        await _userManager.ResetAuthenticatorKeyAsync(user);
        string? unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
    
        if (string.IsNullOrEmpty(unformattedKey))
            return Result<MfaViewModel>.Failure("Failed to get authenticator key");

        await _userManager.UpdateSecurityStampAsync(user);
        
        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
            return Result<MfaViewModel>.Failure("Failed to save authenticator key");
        
        // Enable 2FA
        var result = await _userManager.SetTwoFactorEnabledAsync(user, true);
        if (!result.Succeeded)
            throw new InvalidOperationException("Failed to enable two-factor authentication");

        return Result<MfaViewModel>.Success(new MfaViewModel
        {
            SharedKey = unformattedKey,
            AuthenticatorUri = GenerateQrCodeUri(user.Email!, unformattedKey)
        });
    }

    /// <summary>
    /// Validates the MFA token for the specified user.
    /// </summary>
    /// <param name="user">The user attempting to validate the MFA token.</param>
    /// <param name="token">The MFA token provided by the user.</param>
    /// <returns>A <see cref="Result"/> indicating success or failure.</returns>
    public async Task<Result> ValidateMfa(UserAccountAggregate user, string token)
    {
        // UserAccountAggregate? user = await _userManager.FindByEmailAsync(email);
        //
        // if (user == null)
        //     return Result.Failure("Account does not exist");

        if (!await _userManager.GetTwoFactorEnabledAsync(user))
            return Result.Failure("Invalid 2-factor provider");
        
        var providers = await _userManager.GetValidTwoFactorProvidersAsync(user); 
        if (!providers.Contains(_userManager.Options.Tokens.AuthenticatorTokenProvider))
            return Result.Failure("Invalid 2-factor provider");

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
            
            return updateResult.Succeeded ? Result.Success() : Result.Failure();
            
        }
        
        return Result.Failure("Invalid authenticator code");
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