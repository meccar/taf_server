using Microsoft.AspNetCore.Authorization;
using Shared.Enums;
using Shared.FileObjects;
using Shared.Policies;

namespace Infrastructure.Decorators.Guards;

/// <summary>
/// A custom authorization guard that ensures the user has a company user role.
/// </summary>
public class CompanyUserGuard : AuthorizeAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CompanyUserGuard"/> class.
    /// The guard enforces the company user policy by setting the authorization policy key.
    /// </summary>
    public CompanyUserGuard() : base(new PolicyKey(FoRole.CompanyUser).ToString()) { }
}