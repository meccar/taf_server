using Infrastructure.SeedWork.Enums;
using Infrastructure.SeedWork.Policies;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Decorators.Guards;

public class CompanyUserGuard : AuthorizeAttribute
{
    public CompanyUserGuard() : base(new PolicyKey(ERole.CompanyUser).ToString()) { }
}