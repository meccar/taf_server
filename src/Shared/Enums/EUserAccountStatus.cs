namespace Shared.Enums;

/// <summary>
/// Represents the possible status of a user account.
/// </summary>
public enum EUserAccountStatus
{
    /// <summary>
    /// The user account is active and can perform actions.
    /// </summary>
    Active,

    /// <summary>
    /// The user account is blocked and cannot perform actions.
    /// </summary>
    Blocked,

    /// <summary>
    /// The user account is inactive, likely due to inactivity or suspension.
    /// </summary>
    Inactive
}