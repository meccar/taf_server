using Domain.Aggregates;
using Shared.Model;

namespace Domain.Interfaces.Credentials;

/// <summary>
/// Defines the contract for multi-factor authentication (MFA) operations related to user accounts.
/// This interface includes methods for setting up MFA for a user and validating MFA tokens.
/// </summary>
public interface IMfaRepository
{
    /// <summary>
    /// Sets up multi-factor authentication (MFA) for a specified user account.
    /// This method generates and returns the MFA setup details required for the user to complete MFA setup.
    /// </summary>
    /// <param name="user">The <see cref="UserAccountAggregate"/> representing the user for whom MFA is being set up.</param>
    /// <returns>A <see cref="Task{MfaViewModel}"/> representing the asynchronous operation. The result is an <see cref="MfaViewModel"/> containing the MFA setup details.</returns>
    Task<MfaViewModel?> MfaSetup(UserAccountAggregate user);

    /// <summary>
    /// Validates the multi-factor authentication (MFA) token submitted by a user.
    /// This method checks if the provided token is valid and matches the user's MFA token for authentication.
    /// </summary>
    /// <param name="user">The user attempting to validate the MFA token.</param>
    /// <param name="token">The MFA token provided by the user for validation.</param>
    /// <returns>A <see cref="Task{Result}"/> representing the asynchronous operation. The result contains the outcome of the token validation, indicating whether it is valid or not.</returns>
    Task<bool> ValidateMfa(UserAccountAggregate user, string token);
}
