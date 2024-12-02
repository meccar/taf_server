using Microsoft.AspNetCore.Authorization;
using Shared.Enums;
using Shared.Policies;

namespace Infrastructure.Decorators.Guards;

public class CompanyManagerGuard : AuthorizeAttribute
{
    public CompanyManagerGuard() : base(new PolicyKey(FORole.CompanyManager).ToString()) { }
}