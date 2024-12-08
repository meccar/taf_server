using System.Net;
using Domain.Aggregates;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Shared.Model;
using Shared.Results;

namespace Persistance.Repositories;

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
            SharedKey = FormatKey(unformattedKey!),
            AuthenticatorUri = GenerateQrCodeUri(user.Email!, unformattedKey!)
        };
    }

    /// <summary>
    /// Validates the MFA token for the specified user.
    /// </summary>
    /// <param name="email">The email of the user attempting to validate the MFA token.</param>
    /// <param name="token">The MFA token provided by the user.</param>
    /// <returns>A <see cref="Result"/> indicating success or failure.</returns>
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
    private string FormatKey(string unformattedKey)
    {
        return unformattedKey;
    }
}