using System.Security.Claims;
using Domain.Aggregates;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace Persistance.Repositories.User;

/// <summary>
/// Service responsible for providing user profile data for identity-related operations.
/// </summary>
public class ProfileService : IProfileService
{
    private readonly UserManager<UserAccountAggregate> _userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProfileService"/> class.
    /// </summary>
    /// <param name="userManager">The <see cref="UserManager{UserAccountAggregate}"/> instance used to manage user accounts.</param>
    public ProfileService(
        UserManager<UserAccountAggregate> userManager
    )
    {
        _userManager = userManager;
    }
    
    /// <summary>
    /// Gets the profile data for the authenticated user.
    /// </summary>
    /// <param name="context">The context that contains information about the profile data request.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var userId = context.Subject.FindFirst(JwtClaimTypes.Subject)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            // context.IssuedClaims.Add(new Claim("error", "User ID is missing"));
            return;
        }
        
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            // context.IssuedClaims.Add(new Claim("error", "User not found"));
            return;
        }
        
        // var existingClaims = await _userManager.GetClaimsAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtClaimTypes.Subject, user.EId),
            new Claim(JwtClaimTypes.Email, user.Email!),
        };
        
        context.IssuedClaims.AddRange(claims);
    }

    /// <summary>
    /// Determines whether the user is active.
    /// </summary>
    /// <param name="context">The context that contains information about the user's activity status.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task IsActiveAsync(IsActiveContext context)
    {
        return Task.CompletedTask;
    }
}