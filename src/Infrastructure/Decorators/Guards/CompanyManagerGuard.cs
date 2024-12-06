using Microsoft.AspNetCore.Authorization;
using Shared.Enums;
using Shared.FileObjects;
using Shared.Policies;

namespace Infrastructure.Decorators.Guards;

public class CompanyManagerGuard : AuthorizeAttribute
{
    public CompanyManagerGuard() : base(new PolicyKey(FoRole.CompanyManager).ToString()) { }
}