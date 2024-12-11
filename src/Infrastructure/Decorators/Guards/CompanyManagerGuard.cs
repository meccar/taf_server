using Microsoft.AspNetCore.Authorization;
using Shared.Enums;
using Shared.FileObjects;
using Shared.Policies;

namespace Infrastructure.Decorators.Guards;

/// <summary>
/// A custom authorization guard that ensures the user has a company manager role.
/// </summary>
public class CompanyManagerGuard : AuthorizeAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CompanyManagerGuard"/> class.
    /// The guard enforces the company manager policy by setting the authorization policy key.
    /// </summary>
    public CompanyManagerGuard() : base(new PolicyKey(FoRole.CompanyManager).ToString()) { }
}