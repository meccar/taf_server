using Domain.Aggregates;
using Shared.Model;

namespace Domain.Interfaces.Credentials;

/// <summary>
/// Defines the contract for interacting with email-related operations, specifically for sending email
/// confirmations and verifying email confirmation tokens.
/// </summary>
public interface IMailRepository
{
    /// <summary>
    /// Sends an email confirmation to a user for account verification or multi-factor authentication (MFA).
    /// This method triggers the sending of an email containing a confirmation token to the specified user's account.
    /// </summary>
    /// <param name="userAccount">The <see cref="UserAccountAggregate"/> representing the user's account to whom the email confirmation will be sent.</param>
    /// <param name="mfaViewModel">The <see cref="MfaViewModel"/> containing the multi-factor authentication data needed for the confirmation email.</param>
    /// <returns>A <see cref="Task{Result}"/> representing the asynchronous operation. The result contains the status of the email sending operation.</returns>
    Task<bool> SendEmailConfirmation(UserAccountAggregate userAccount, MfaViewModel mfaViewModel);

    /// <summary>
    /// Verifies the provided email confirmation token to validate its authenticity and expiration status.
    /// This method is used to check if a given token is valid for confirming a user's email address.
    /// </summary>
    /// <param name="token">The confirmation token to be verified.</param>
    /// <returns>A <see cref="string"/> that returns the result of the verification. If valid, returns the user identifier or null if invalid.</returns>
    Task<UserAccountAggregate?> VerifyEmailConfirmationToken(UserAccountAggregate user, string userKey, string emailKey);
}