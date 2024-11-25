using Infrastructure.SeedWork.Enums;
using Infrastructure.SeedWork.Policies;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Decorators.Guards;

public class UserGuard : AuthorizeAttribute
{
    public UserGuard() : base(new PolicyKey(ERole.User).ToString()) { }
}