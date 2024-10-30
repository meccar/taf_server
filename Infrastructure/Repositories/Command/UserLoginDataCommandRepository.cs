using AutoMapper;
using Domain.Aggregates;
using Domain.Interfaces.Command;
using Domain.Model;
using Infrastructure.Data;
using Infrastructure.Entities;
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
    : RepositoryBase<UserLoginDataEntity>, IUserLoginDataCommandRepository
{
    private readonly IMapper _mapper;
    private readonly UserManager<UserAccountAggregate> _userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserLoginDataCommandRepository"/> class.
    /// </summary>
    /// <param name="context">The database context used for data operations.</param>
    /// <param name="mapper">The AutoMapper instance for object mapping.</param>
    /// <param name="userManager">The UserManager for managing user login data.</param>
    public UserLoginDataCommandRepository(
        ApplicationDbContext context,
        IMapper mapper,
        UserManager<UserAccountAggregate> userManager)
        : base(context)
    {
        _mapper = mapper;
        _userManager = userManager;
    }

    /// <summary>
    /// Creates a new user login data entry based on the provided DTO.
    /// </summary>
    /// <param name="userLoginDataDto">The DTO containing user login data details.</param>
    /// <returns>The created user login data model.</returns>
    public async Task<UserLoginDataModel> CreateUserLoginData(UserLoginDataModel userLoginDataDto)
    {
        var userLoginDataEntity = _mapper.Map<UserLoginDataEntity>(userLoginDataDto);

        // userLoginDataEntity.PasswordHash = HashHelper.Encrypt(userLoginDataDto.Password);
        await CreateAsync(userLoginDataEntity);

        var userLoginDataModel = _mapper.Map<UserLoginDataModel>(userLoginDataEntity);
        return userLoginDataModel;
    }
}