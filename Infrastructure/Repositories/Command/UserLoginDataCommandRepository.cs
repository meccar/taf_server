using AutoMapper;
using Microsoft.AspNetCore.Identity;
using taf_server.Domain.Aggregates;
using taf_server.Domain.Interfaces;
using taf_server.Domain.Model;
using taf_server.Domain.Repositories;
using taf_server.Infrastructure.Data;
using taf_server.Infrastructure.Entities;
using taf_server.Presentations.Dtos.UserLoginData;
using taf_server.Presentations.Helper;

namespace taf_server.Infrastructure.Repositories.Command;

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
    /// Checks if a user login data with the specified credential exists.
    /// </summary>
    /// <param name="loginCredential">The login credential to check, such as an email.</param>
    /// <returns><c>true</c> if the user login data exists; otherwise, <c>false</c>.</returns>
    public async Task<bool> IsUserLoginDataExisted(string userLoginData)
    {
        return await ExistAsync(u => u.Email == userLoginData);
    }

    /// <summary>
    /// Creates a new user login data entry based on the provided DTO.
    /// </summary>
    /// <param name="userLoginDataDto">The DTO containing user login data details.</param>
    /// <returns>The created user login data model.</returns>
    public async Task<UserLoginDataModel> CreateUserLoginData(CreateUserLoginDataDto userLoginDataDto)
    {
        var userLoginDataEntity = _mapper.Map<UserLoginDataEntity>(userLoginDataDto);

        userLoginDataEntity.PasswordHash = HashHelper.Encrypt(userLoginDataDto.Password);
        await CreateAsync(userLoginDataEntity);

        var userLoginDataModel = _mapper.Map<UserLoginDataModel>(userLoginDataEntity);
        return userLoginDataModel;
    }
}