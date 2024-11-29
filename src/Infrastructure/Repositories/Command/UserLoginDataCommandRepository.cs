using System.Security.Claims;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Command;
using Domain.Interfaces.Service;
using Domain.Model;
using Domain.SeedWork.Results;
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
    private readonly IMfaService _mfaService;

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
        UserManager<UserLoginDataEntity> userManager,
        IMfaService mfaService

        // RoleManager<IdentityRole> roleManager
        )
    {
        _mapper = mapper;
        _userManager = userManager;
        _mfaService = mfaService;
        // _roleManager = roleManager;
    }

    /// <summary>
    /// Creates a new user login data entry based on the provided DTO.
    /// </summary>
    /// <param name="userLoginDataDto">The DTO containing user login data details.</param>
    /// <returns>The created user login data model.</returns>
    public async Task<UserLoginDataResult> CreateUserLoginDataAsync(UserLoginDataModel request)
    {
        var (userLoginDataEntity, createResult) = await CreateUserAccountAsync(request);
        if (!createResult.Succeeded)
        {
            return UserLoginDataResult.Failure(
                createResult.Errors.Select(e => e.Description).ToArray());
        }
        
        var roleResult = await AssignRoleAsync(userLoginDataEntity);
        if (!roleResult.Succeeded)
        {
            return UserLoginDataResult.Failure(
                roleResult.Errors.Select(e => e.Description).ToArray());
        }

        var userLoginDataModel = _mapper.Map<UserLoginDataModel>(userLoginDataEntity);
        return UserLoginDataResult.Success(userLoginDataModel);
        // if (await _mfaService.MfaSetup(userLoginDataEntity))
        // {
        // }

        // return UserLoginDataResult.Failure(
        //     createResult.Errors.Select(e => e.Description).ToArray());
    }
    
    private async Task<(UserLoginDataEntity User, IdentityResult Result)> CreateUserAccountAsync(
        UserLoginDataModel request)
    {
        var userEntity = _mapper.Map<UserLoginDataEntity>(request);
        userEntity.UserName ??= userEntity.Email;

        var result = await _userManager.CreateAsync(userEntity, request.Password);
        request.Password = null;

        return (userEntity, result);
    }
    
    private async Task<IdentityResult> AssignRoleAsync(UserLoginDataEntity user)
    {
        return await _userManager.AddToRoleAsync(user, ERole.User);
    }
}