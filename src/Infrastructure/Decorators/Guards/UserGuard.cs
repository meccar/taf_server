using Microsoft.AspNetCore.Authorization;
using Shared.Enums;
using Shared.FileObjects;
using Shared.Policies;

namespace Infrastructure.Decorators.Guards;

/// <summary>
/// A custom authorization guard that ensures the user has a user role.
/// </summary>
public class UserGuard : AuthorizeAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserGuard"/> class.
    /// The guard enforces the user policy by setting the authorization policy key.
    /// </summary>
    public UserGuard() : base(new PolicyKey(FoRole.User).ToString()) { }
}