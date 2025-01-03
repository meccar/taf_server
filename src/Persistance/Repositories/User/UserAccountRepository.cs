using Domain.Aggregates;
using Domain.Interfaces.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Repositories.User;

/// <summary>
/// Repository for managing user accounts, including creation, validation, and login data checks.
/// </summary>
public class UserAccountRepository
    : IUserAccountRepository
{
    private readonly UserManager<UserAccountAggregate> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserAccountRepository"/> class.
    /// </summary>
    /// <param name="userManager">The UserManager for managing user accounts.</param>
    /// <param name="mfaRepository">The repository responsible for MFA setup.</param>
    /// <param name="mailRepository">The repository responsible for sending confirmation emails.</param>
    public UserAccountRepository(
        UserManager<UserAccountAggregate> userManager,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IdentityResult> CreateAsync(UserAccountAggregate userAccountAggregate, string password)
    {
        userAccountAggregate.UserName ??= userAccountAggregate.Email;
        return await _userManager.CreateAsync(userAccountAggregate, password);
    }
    
    public async Task<IdentityResult> AddToRoleAsync(UserAccountAggregate userAccountAggregate, string role)
    {
        return await _userManager.AddToRoleAsync(userAccountAggregate, role);
    }
    
    public async Task<IdentityResult> UpdateAsync(UserAccountAggregate userAccountAggregate)
    {
        return await _userManager.UpdateAsync(userAccountAggregate);
    }
    
    /// <summary>
    /// Checks whether a user with the same login data (email or phone number) already exists.
    /// </summary>
    /// <param name="userLoginDataModel">The user account model to check for existing login data.</param>
    /// <returns>True if the login data already exists; otherwise, false.</returns>
    public async Task<bool> IsUserLoginDataExisted(UserAccountAggregate userAccountAggregate)
    {
        bool email = await _userManager.Users
            .AsQueryable()
            .AnyAsync(
                u =>
                    u.Email == userAccountAggregate.Email);
        
        bool phone = await _userManager.Users
            .AsQueryable()
            .AnyAsync(
                u =>
                    u.PhoneNumber == userAccountAggregate.PhoneNumber);
        
        return email || phone;
    }
    
    public async Task<bool> IsUserLoginDataExisted(string userLoginData)
    {
        bool result = await _userManager.Users
            .AsQueryable()
            .AnyAsync(
                u =>
                    u.Email == userLoginData || u.PhoneNumber == userLoginData);
        
        return result;
    }

    public async Task<UserAccountAggregate?> IsExistingAndVerifiedUserAccount(string Eid)
    {
        return await _userManager
                        .Users
                        .AsQueryable()
                        .FirstOrDefaultAsync(
                            u 
                                => u.EId == Eid 
                            && u.EmailConfirmed
                        );
    }

    public async Task<UserAccountAggregate?> GetUserByEmail(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }
    
    public async Task<UserAccountAggregate?> GetCurrentUser()
    {
        var result = _httpContextAccessor.HttpContext?.User;
        return result != null 
            ? await _userManager.GetUserAsync(result)
            : null;
    }
    
    public async Task<UserAccountAggregate?> GetCurrentUser(string eid)
    {
        var result = _httpContextAccessor.HttpContext?.User;
        
        if (result == null) return null;
        
        var user = await _userManager.GetUserAsync(result);
        
        return user!.EId == eid ? user : null;
    }
}