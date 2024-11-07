using System.Security.Claims;
using Domain.Entities;
using Infrastructure.SeedWork.Enums;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data;

public class ApplicationDbContextSeed
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<UserLoginDataEntity> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    
    public ApplicationDbContextSeed(
        ApplicationDbContext context,
        UserManager<UserLoginDataEntity> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        // SeedUserLoginDataEntityAsync().Wait();
        if (!_context.Roles.Any())
            await SeedRoleAsync();
    }

    // private async Task SeedUserLoginDataEntityAsync()
    // {
    //     if (!_userManager.Users.Any())
    //     {
    //         var admin = new UserLoginDataEntity
    //         {
    //             FirstName = "Testo",
    //             LastName = "Testla",
    //             Gender = Gender.Male,
    //             DateOfBirth = DateTime.Now,
    //             PhoneNumber = "088888888888",
    //             Avatar = "avatar.jpg",
    //             CreatedAt = DateTime.Now,
    //         };
    //         
    //         var result = await _userManager.CreateAsync(admin, "Admin@123456789");
    //         if (!result.Succeeded)
    //         {
    //             var user = await _userManager.FindByEmailAsync(admin);
    //             if(user != null)
    //                 await _userManager.AddToRoleAsync(user, "Admin");
    //         }
    //     }
    //     
    //     await _context.SaveChangesAsync();
    // }

    private async Task SeedRoleAsync()
    {
        var roles = new[]
        {
            new { Name = "Admin", Claims = new[] { new Claim(EClaimTypes.Permission, "projects.view") } },
            new { Name = "CompanyManager", Claims = new Claim[0] },
            new { Name = "CompanyUser", Claims = new Claim[0] },
            new { Name = "User", Claims = new Claim[0] },
            new { Name = "Guest", Claims = new Claim[0] }
        };
        
        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role.Name))
            {
                var newRole = new IdentityRole<Guid>(role.Name);
                await _roleManager.CreateAsync(newRole);
                
                if (role.Claims.Length > 0)
                {
                    var createdRole = await _roleManager.FindByNameAsync(role.Name);
                    foreach (var claim in role.Claims)
                    {
                        await _roleManager.AddClaimAsync(createdRole, claim);
                    }
                }
            }
        }
    }
}