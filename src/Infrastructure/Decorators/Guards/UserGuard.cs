using Microsoft.AspNetCore.Authorization;
using Shared.Enums;
using Shared.FileObjects;
using Shared.Policies;

namespace Infrastructure.Decorators.Guards;

public class UserGuard : AuthorizeAttribute
{
    public UserGuard() : base(new PolicyKey(FoRole.User).ToString()) { }
}