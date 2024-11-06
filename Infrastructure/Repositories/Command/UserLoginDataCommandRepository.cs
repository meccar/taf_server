using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Command;
using Domain.Model;
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
    
    /// <summary>
    /// Initializes a new instance of the <see cref="UserLoginDataCommandRepository"/> class.
    /// </summary>
    /// <param name="context">The database context used for data operations.</param>
    /// <param name="mapper">The AutoMapper instance for object mapping.</param>
    /// <param name="userManager">The UserManager for managing user login data.</param>
    public UserLoginDataCommandRepository(
        // ApplicationDbContext context,
        IMapper mapper,
        UserManager<UserLoginDataEntity> userManager)
    {
        _mapper = mapper;
        _userManager = userManager;
    }

    /// <summary>
    /// Creates a new user login data entry based on the provided DTO.
    /// </summary>
    /// <param name="userLoginDataDto">The DTO containing user login data details.</param>
    /// <returns>The created user login data model.</returns>
    public async Task<UserLoginDataModel> CreateUserLoginDataAsync(UserLoginDataModel request)
    {
        var userLoginDataEntity = _mapper.Map<UserLoginDataEntity>(request);
        
        if (string.IsNullOrEmpty(userLoginDataEntity.UserName))
            userLoginDataEntity.UserName = userLoginDataEntity.Email;
        
        var result = await _userManager.CreateAsync(userLoginDataEntity, request.Password);

        if (result.Succeeded)
        {
            // await _userManager.AddToRoleAsync(userLoginDataEntity, "User");
            
            var userLoginDataModel = _mapper.Map<UserLoginDataModel>(userLoginDataEntity);
            
            return userLoginDataModel;
            
        }
        var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
        throw new InvalidOperationException($"Failed to create user login data: {errorMessages}");
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