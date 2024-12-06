namespace Shared.Enums;

/// <summary>
/// Enumeration for different email statuses used in the application.
/// </summary>
public enum EEmailStatus
{
    /// <summary>
    /// Represents an email that is pending verification or processing.
    /// </summary>
    Pending,

    /// <summary>
    /// Represents a valid email that has been successfully verified.
    /// </summary>
    Valid,

    /// <summary>
    /// Represents an email that is invalid or cannot be verified.
    /// </summary>
    Invalid,

    /// <summary>
    /// Represents an email that has expired, typically after a certain period of time.
    /// </summary>
    Expired,

    /// <summary>
    /// Represents an error in email verification or processing.
    /// </summary>
    Error
}