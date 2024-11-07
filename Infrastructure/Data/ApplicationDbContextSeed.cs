using Domain.Entities;
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
        var roles = new[] { "Admin", "CompanyManager", "CompanyUser", "User", "Guest" };
        
        foreach (var roleName in roles)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                
            }
        }
    }
}