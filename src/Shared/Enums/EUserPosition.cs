namespace Shared.Enums;

/// <summary>
/// Represents the different positions a user can hold within a system.
/// </summary>
public enum EUserPosition
{
    /// <summary>
    /// A general user with no special role or privileges.
    /// </summary>
    USER,

    /// <summary>
    /// A user associated with a company, typically with limited access to certain resources.
    /// </summary>
    COMPANY_USER,

    /// <summary>
    /// A manager role within a company, with additional permissions over company users and resources.
    /// </summary>
    COMPANY_MANAGER,

    /// <summary>
    /// An admin with the highest level of access and control over the system.
    /// </summary>
    ADMIN
}