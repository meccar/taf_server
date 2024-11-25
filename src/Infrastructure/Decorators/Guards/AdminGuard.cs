using Infrastructure.SeedWork.Enums;
using Infrastructure.SeedWork.Policies;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Decorators.Guards;

public class AdminGuard : AuthorizeAttribute
{
    public AdminGuard() : base(new PolicyKey(ERole.Admin).ToString()) { }
}