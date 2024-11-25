using Infrastructure.SeedWork.Enums;
using Infrastructure.SeedWork.Policies;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Decorators.Guards;

public class CompanyManagerGuard : AuthorizeAttribute
{
    public CompanyManagerGuard() : base(new PolicyKey(ERole.CompanyManager).ToString()) { }
}