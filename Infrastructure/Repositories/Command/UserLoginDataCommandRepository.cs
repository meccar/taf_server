using System.Security.Claims;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Command;
using Domain.Model;
using Infrastructure.SeedWork.Enums;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories.Command;

/// <summary>
/// Represents the command repository for managing user login data entities and operations.
/// </summary>
/// <remarks>
/// This class provides methods to interact with user login data, including creating new user login data 
/// and checking for existing login credentials. It leverages AutoMapper for object mapping and 
/// <see cref="UserManager{TUser}"/> for user management tasks.
/// </remarks>
public class UserLoginDataCommandRepository
    : IUserLoginDataCommandRepository
{
    private readonly IMapper _mapper;
    private readonly UserManager<UserLoginDataEntity> _userManager;
    // private readonly RoleManager<IdentityRole> _roleManager;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="UserLoginDataCommandRepository"/> class.
    /// </summary>
    /// <param name="context">The database context used for data operations.</param>
    /// <param name="mapper">The AutoMapper instance for object mapping.</param>
    /// <param name="userManager">The UserManager for managing user login data.</param>
    public UserLoginDataCommandRepository(
        // ApplicationDbContext context,
        IMapper mapper,
        UserManager<UserLoginDataEntity> userManager
        // RoleManager<IdentityRole> roleManager
        )
    {
        _mapper = mapper;
        _userManager = userManager;
        // _roleManager = roleManager;
    }

    /// <summary>
    /// Creates a new user login data entry based on the provided DTO.
    /// </summary>
    /// <param name="userLoginDataDto">The DTO containing user login data details.</param>
    /// <returns>The created user login data model.</returns>
    public async Task<UserLoginDataModel?> CreateUserLoginDataAsync(UserLoginDataModel request)
    {
        var userLoginDataEntity = _mapper.Map<UserLoginDataEntity>(request);
        
        if (string.IsNullOrEmpty(userLoginDataEntity.UserName))
            userLoginDataEntity.UserName = userLoginDataEntity.Email;
        
        // var roleCreationResult = await _roleManager.CreateAsync(new IdentityRole("User"));
        //
        // if (!roleCreationResult.Succeeded)
        // {
        //     var roleErrors = string.Join(", ", roleCreationResult.Errors.Select(e => e.Description));
        //     throw new InvalidOperationException($"Failed to create role: {roleErrors}");
        // }
        
        var userAccountCreationResult = await _userManager.CreateAsync(userLoginDataEntity, request.Password);
        request.Password = null;
        
        
        if (userAccountCreationResult.Succeeded)
        {
            var roleCreationResult = await _userManager.AddToRoleAsync(userLoginDataEntity, ERole.User);
            if (!roleCreationResult.Succeeded)
            {
                return null;
            }
            
            // var userClaims = ERoleWithClaims.RoleClaims[ERole.User];
            //
            // foreach (var claim in userClaims)
            // {
            //     var claimToAdd = new Claim(EClaimTypes.Permission, claim.ToString());
            //
            //     await _userManager.AddClaimAsync(userLoginDataEntity, claimToAdd);
            // }
            //
            // if (!claimResult.Succeeded)
            // {
            //     var claimErrors = string.Join(", ", claimResult.Errors.Select(e => e.Description));
            //     throw new InvalidOperationException($"Failed to add claim to user: {claimErrors}");
            // }
            
            var userLoginDataModel = _mapper.Map<UserLoginDataModel>(userLoginDataEntity);
            
            return userLoginDataModel;
            
        }
        return null;
    }
    
    
    /// <summary>
    /// Adds the specified user to the provided roles.
    /// </summary>
    /// <param name="user">The user account to add roles to.</param>
    /// <param name="roles">The roles to assign to the user.</param>
    public async Task AddUserToRolesAsync(UserLoginDataEntity user, IEnumerable<string> roles)
    {
        await _userManager.AddToRolesAsync(user, roles);
    }
}