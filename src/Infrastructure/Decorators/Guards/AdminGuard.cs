using Microsoft.AspNetCore.Authorization;
using Shared.Enums;
using Shared.FileObjects;
using Shared.Policies;

namespace Infrastructure.Decorators.Guards;

/// <summary>
/// A custom authorization guard that ensures the user has an admin role.
/// </summary>
public class AdminGuard : AuthorizeAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AdminGuard"/> class.
    /// The guard enforces the admin policy by setting the authorization policy key.
    /// </summary>
    public AdminGuard() : base(new PolicyKey(FoRole.Admin).ToString()) { }
}