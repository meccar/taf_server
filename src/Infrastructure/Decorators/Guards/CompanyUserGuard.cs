using Microsoft.AspNetCore.Authorization;
using Shared.Enums;
using Shared.FileObjects;
using Shared.Policies;

namespace Infrastructure.Decorators.Guards;

public class CompanyUserGuard : AuthorizeAttribute
{
    public CompanyUserGuard() : base(new PolicyKey(FoRole.CompanyUser).ToString()) { }
}