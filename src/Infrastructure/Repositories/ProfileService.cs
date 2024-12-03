using System.Security.Claims;
using Domain.Entities;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories;

public class ProfileService : IProfileService
{
    private readonly UserManager<UserAccountAggregate> _userManager;

    public ProfileService(
        UserManager<UserAccountAggregate> userManager
    )
    {
        _userManager = userManager;
    }
    
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var userId = context.Subject?.FindFirst(IdentityModel.JwtClaimTypes.Subject)?.Value;
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
        
        var existingClaims = await _userManager.GetClaimsAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtClaimTypes.Subject, user.EId),
            new Claim(JwtClaimTypes.Email, user.Email),
        };
        
        context.IssuedClaims.AddRange(claims);
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        return Task.CompletedTask;
    }
}