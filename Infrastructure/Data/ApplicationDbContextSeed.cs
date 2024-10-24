// using Microsoft.AspNetCore.Identity;
// using taf_server.Domain.Aggregates;
// using taf_server.Domain.SeedWork.Enums.UserAccount;
//
// namespace taf_server.Infrastructure.Data;
//
// public class ApplicationDbContextSeed
// {
//     private readonly ApplicationDbContext _context;
//     private readonly UserManager<UserAccountAggregate> _userManager;
//     private readonly RoleManager<RoleAggregate> _roleManager;
//     
//     public ApplicationDbContextSeed(
//         ApplicationDbContext context,
//         UserManager<UserAccountAggregate> userManager,
//         RoleManager<RoleAggregate> roleManager)
//     {
//         _context = context;
//         _userManager = userManager;
//         _roleManager = roleManager;
//     }
//
//     public async Task SeedAsync()
//     {
//         SeedUserAccountAsync().Wait();
//     }
//
//     private async Task SeedUserAccountAsync()
//     {
//         if (!_userManager.Users.Any())
//         {
//             var admin = new UserAccountAggregate
//             {
//                 FirstName = "Testo",
//                 LastName = "Testla",
//                 Gender = Gender.Male,
//                 DateOfBirth = DateTime.Now,
//                 PhoneNumber = "088888888888",
//                 Avatar = "avatar.jpg",
//                 CreatedAt = DateTime.Now,
//             };
//             
//             var result = await _userManager.CreateAsync(admin, "Admin@123456789");
//             if (!result.Succeeded)
//             {
//                 var user = await _userManager.FindByEmailAsync(admin);
//                 if(user != null)
//                     await _userManager.AddToRoleAsync(user, "Admin");
//             }
//         }
//         
//         await _context.SaveChangesAsync();
//     }
//
//     // private async Task SeedUserLoginDataAsync()
//     // {
//     //     if (!_userManager.Users.Any())
//     //     {
//     //         
//     //     }
//     // }
// }