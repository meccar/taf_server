using System.Security.Claims;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configurations.Identity;
public static class IdentityConfiguration
{
    public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
    {
        services
            .AddIdentity<UserLoginDataEntity, IdentityRole<int>>(options =>
            {
                // // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 12;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                // options.User.AllowedUserNameCharacters = "";
                // options.Stores.ProtectPersonalData = true;
                options.User.RequireUniqueEmail = true;

                // ClaimsIdentity
                options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;
                options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
                options.ClaimsIdentity.SecurityStampClaimType = ClaimTypes.System;

                // options.SignIn.RequireConfirmedPhoneNumber = true;
                // options.SignIn.RequireConfirmedEmail = true;

                // Tokens
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultProvider;
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
                options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
            })
            .AddRoles<IdentityRole<int>>()
            .AddRoleManager<RoleManager<IdentityRole<int>>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
            // .AddErrorDescriber<CustomIdentityErrorDescriber>();/
        
            // services.AddSingleton<ILookupProtectorKeyRing, KeyRing>();
            // services.AddSingleton<ILookupProtector, LookupProtector>();
            // services.AddSingleton<IPersonalDataProtector, PersonalDataProtector>();
        
        //.AddTokenProvider<>()
        
        return services;
    }
}
