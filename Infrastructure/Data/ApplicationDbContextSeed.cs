using System.Security.Claims;
using Domain.Entities;
using Infrastructure.SeedWork.Enums;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data;

public class ApplicationDbContextSeed
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<UserLoginDataEntity> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    private static readonly IReadOnlyDictionary<string, IReadOnlyCollection<string>> DefaultPermissions = 
        new Dictionary<string, IReadOnlyCollection<string>>
        {
            { 
                ERole.Admin, 
                new[] 
                {
                    EClaimValue.View,
                    EClaimValue.Read,
                    EClaimValue.Update,
                    EClaimValue.Delete
                }
            },
            { 
                ERole.CompanyManager, 
                new[] 
                {
                    EClaimValue.View,
                    EClaimValue.Read,
                    EClaimValue.Update
                }
            },
            { 
                ERole.CompanyUser, 
                new[] 
                {
                    EClaimValue.View,
                    EClaimValue.Read,
                    EClaimValue.Update
                }
            },
            { 
                ERole.User, 
                new[] 
                {
                    EClaimValue.View,
                    EClaimValue.Read,
                    EClaimValue.Update
                }
            },
        };
    
    public ApplicationDbContextSeed(
        ApplicationDbContext context,
        UserManager<UserLoginDataEntity> userManager,
        RoleManager<IdentityRole<int>> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        await SeedRolesAsync();
    }

    private async Task SeedRolesAsync()
    {
            
        foreach (var (roleName, permissions) in DefaultPermissions)
        {
            var role = await CreateRoleAsync(roleName);
            if (role != null)
            {
                await UpdateRoleClaimsAsync(role, permissions);
            }
        }
    }
    
    private async Task<IdentityRole<int>?> CreateRoleAsync(string roleName)
    {
        var existingRole = await _roleManager.FindByNameAsync(roleName);
        if (existingRole != null)
        {
            return existingRole;
        }

        var newRole = new IdentityRole<int>(roleName);
        var result = await _roleManager.CreateAsync(newRole);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(
                $"Failed to create role {roleName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return newRole;
    }
    
    private async Task UpdateRoleClaimsAsync(
        IdentityRole<int> role,
        IEnumerable<string> permissions)
    {
        var existingClaims = await _roleManager.GetClaimsAsync(role);
        var desiredClaims = permissions.Select(p => new Claim(EClaimTypes.Permission, p));

        await RemoveOutdatedClaimsAsync(role, existingClaims, desiredClaims);
        await AddNewClaimsAsync(role, existingClaims, desiredClaims);
    }
    
    private async Task RemoveOutdatedClaimsAsync(
        IdentityRole<int> role,
        IList<Claim> existingClaims,
        IEnumerable<Claim> desiredClaims)
    {
        var claimsToRemove = existingClaims
            .Where(claim => !desiredClaims.Any(c =>
                c.Type == claim.Type && c.Value == claim.Value))
            .ToList();

        foreach (var claim in claimsToRemove)
        {
            var result = await _roleManager.RemoveClaimAsync(role, claim);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Failed to remove claim {claim.Value} from role {role.Name}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
    }
    
    private async Task AddNewClaimsAsync(
        IdentityRole<int> role,
        IList<Claim> existingClaims,
        IEnumerable<Claim> desiredClaims)
    {
        var claimsToAdd = desiredClaims
            .Where(claim => !existingClaims.Any(c =>
                c.Type == claim.Type && c.Value == claim.Value))
            .ToList();

        foreach (var claim in claimsToAdd)
        {
            var result = await _roleManager.AddClaimAsync(role, claim);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Failed to add claim {claim.Value} to role {role.Name}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
    }
}