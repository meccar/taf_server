namespace Shared.FileObjects;

/// <summary>
/// Represents predefined role constants used within the system.
/// </summary>
public class FoRole
{
    /// <summary>
    /// The Admin role, with the highest level of access and privileges.
    /// </summary>
    public const string Admin = "Admin";

    /// <summary>
    /// The CompanyManager role, typically responsible for managing users and resources within a company.
    /// </summary>
    public const string CompanyManager = "CompanyManager";

    /// <summary>
    /// The CompanyUser role, typically a regular user within a company with limited access.
    /// </summary>
    public const string CompanyUser = "CompanyUser";

    /// <summary>
    /// The User role, a general user with basic access to the system.
    /// </summary>
    public const string User = "User";
}