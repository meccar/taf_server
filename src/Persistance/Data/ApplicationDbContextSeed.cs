using System.Security.Claims;
using Domain.Aggregates;
using Microsoft.AspNetCore.Identity;
using Shared.Enums;
using Shared.FileObjects;

namespace Persistance.Data;

public class ApplicationDbContextSeed
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<UserAccountAggregate> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    private static readonly IReadOnlyDictionary<string, IReadOnlyCollection<string>> DefaultPermissions = 
        new Dictionary<string, IReadOnlyCollection<string>>
        {
            { 
                FORole.Admin, 
                new[] 
                {
                    FOClaimeActionsValue.View,
                    FOClaimeActionsValue.Read,
                    FOClaimeActionsValue.Update,
                    FOClaimeActionsValue.Delete
                }
            },
            { 
                FORole.CompanyManager, 
                new[] 
                {
                    FOClaimeActionsValue.View,
                    FOClaimeActionsValue.Read,
                    FOClaimeActionsValue.Update
                }
            },
            { 
                FORole.CompanyUser, 
                new[] 
                {
                    FOClaimeActionsValue.View,
                    FOClaimeActionsValue.Read,
                    FOClaimeActionsValue.Update
                }
            },
            { 
                FORole.User, 
                new[] 
                {
                    FOClaimeActionsValue.View,
                    FOClaimeActionsValue.Read,
                    FOClaimeActionsValue.Update
                }
            },
        };
    
    public ApplicationDbContextSeed(
        ApplicationDbContext context,
        UserManager<UserAccountAggregate> userManager,
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
        var desiredClaims = permissions.Select(p => new Claim(EClaimTypes.Permission.ToString(), p));

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