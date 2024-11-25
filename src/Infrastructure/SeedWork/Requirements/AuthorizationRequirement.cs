using Infrastructure.SeedWork.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.SeedWork.Requirements;

public class AuthorizationRequirement : IAuthorizationRequirement
{
    public ERole Role { get; }
    // public EDepartment? Department { get; }
    // public EAccessLevel? MinimumLevel { get; }

    public AuthorizationRequirement(ERole role)
    {
        Role = role;
    }

    // public RoleRequirement(ERole role, EDepartment department)
    // {
    //     Role = role;
    //     Department = department;
    // }
    //
    // public RoleRequirement(ERole role, EDepartment department, EAccessLevel minimumLevel)
    // {
    //     Role = role;
    //     Department = department;
    //     MinimumLevel = minimumLevel;
    // }
}