using Microsoft.AspNetCore.Authorization;
using Shared.Enums;
using Shared.FileObjects;
using Shared.Policies;

namespace Infrastructure.Decorators.Guards;

public class AdminGuard : AuthorizeAttribute
{
    public AdminGuard() : base(new PolicyKey(FoRole.Admin).ToString()) { }
}