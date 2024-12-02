using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Command;
using Domain.Interfaces.Service;
using Domain.SeedWork.Results;
using Microsoft.AspNetCore.Identity;
using Shared.Enums;
using Shared.Model;

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
    private readonly UserManager<UserAccountAggregate> _userManager;
    private readonly IMfaRepository _mfaRepository;

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
        UserManager<UserAccountAggregate> userManager,
        IMfaRepository mfaRepository

        // RoleManager<IdentityRole> roleManager
        )
    {
        _mapper = mapper;
        _userManager = userManager;
        _mfaRepository = mfaRepository;
        // _roleManager = roleManager;
    }

    /// <summary>
    /// Creates a new user login data entry based on the provided DTO.
    /// </summary>
    /// <param name="userLoginDataDto">The DTO containing user login data details.</param>
    /// <returns>The created user login data model.</returns>
    public async Task<UserLoginDataResult> CreateUserLoginDataAsync(UserLoginDataModel request)
    {
        var (userAccountAggregate, createResult) = await CreateUserAccountAsync(request);
        if (!createResult.Succeeded)
        {
            return UserLoginDataResult.Failure(
                createResult.Errors.Select(e => e.Description).ToArray());
        }
        
        var roleResult = await AssignRoleAsync(userAccountAggregate);
        if (!roleResult.Succeeded)
        {
            return UserLoginDataResult.Failure(
                roleResult.Errors.Select(e => e.Description).ToArray());
        }

        var userLoginDataModel = _mapper.Map<UserLoginDataModel>(userAccountAggregate);
        return UserLoginDataResult.Success(userLoginDataModel);
        // if (await _mfaRepository.MfaSetup(userAccountAggregate))
        // {
        // }

        // return UserLoginDataResult.Failure(
        //     createResult.Errors.Select(e => e.Description).ToArray());
    }
    
    private async Task<(UserAccountAggregate User, IdentityResult Result)> CreateUserAccountAsync(
        UserLoginDataModel request)
    {
        var userEntity = _mapper.Map<UserAccountAggregate>(request);
        userEntity.UserName ??= userEntity.Email;

        var result = await _userManager.CreateAsync(userEntity, request.Password);
        request.Password = null;

        return (userEntity, result);
    }
    
    private async Task<IdentityResult> AssignRoleAsync(UserAccountAggregate user)
    {
        return await _userManager.AddToRoleAsync(user, FORole.User);
    }
}