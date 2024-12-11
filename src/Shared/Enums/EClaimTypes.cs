namespace Shared.Enums;

/// <summary>
/// Enumeration for different types of claims used in the application.
/// </summary>
public enum EClaimTypes
{
    /// <summary>
    /// Represents a claim for the user's permissions.
    /// </summary>
    Permission,

    /// <summary>
    /// Represents a claim for the user's email address.
    /// </summary>
    Email,

    /// <summary>
    /// Represents a claim for the user's subscription level.
    /// </summary>
    SubscriptionLevel,

    /// <summary>
    /// Represents a claim for the user's role.
    /// </summary>
    Role
}
