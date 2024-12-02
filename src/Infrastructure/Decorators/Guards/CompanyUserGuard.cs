using Microsoft.AspNetCore.Authorization;
using Shared.Enums;
using Shared.Policies;

namespace Infrastructure.Decorators.Guards;

public class CompanyUserGuard : AuthorizeAttribute
{
    public CompanyUserGuard() : base(new PolicyKey(FORole.CompanyUser).ToString()) { }
}