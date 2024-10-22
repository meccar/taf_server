using AutoMapper;
using Microsoft.AspNetCore.Identity;
using taf_server.Domain.Aggregates;
using taf_server.Domain.Interfaces;
using taf_server.Domain.Model;
using taf_server.Domain.Repositories;
using taf_server.Infrastructure.Data;
using taf_server.Infrastructure.Entities;
using taf_server.Presentations.Dtos.UserAccount;

namespace taf_server.Infrastructure.Repositories.Command;

/// <summary>
/// Represents the command repository for managing user account entities and operations.
/// </summary>
/// <remarks>
/// This class provides methods to interact with user account data, including creating new user accounts 
/// and checking for existing user account data. It leverages AutoMapper for object mapping and 
/// <see cref="UserManager{TUser}"/> for user management tasks.
/// </remarks>
public class UserAccountCommandRepository 
    : RepositoryBase<UserAccountEntity>, IUserAccountCommandRepository
{
    private readonly IMapper _mapper;
    private readonly UserManager<UserAccountAggregate> _userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserAccountCommandRepository"/> class.
    /// </summary>
    /// <param name="context">The database context used for data operations.</param>
    /// <param name="mapper">The AutoMapper instance for object mapping.</param>
    /// <param name="userManager">The UserManager for managing user accounts.</param>
    public UserAccountCommandRepository(
        ApplicationDbContext context,
        IMapper mapper,
        UserManager<UserAccountAggregate> userManager)
            : base(context)
    {
        _mapper = mapper;
        _userManager = userManager;
    }

    /// <summary>
    /// Checks if a user account with the specified data exists.
    /// </summary>
    /// <param name="userAccountData">The user account data to check, such as a phone number.</param>
    /// <returns><c>true</c> if the user account exists; otherwise, <c>false</c>.</returns>
    public async Task<bool> IsUserAccountDataExisted(string userAccountData)
    {
        return await ExistAsync(u => u.PhoneNumber == userAccountData);
    }

    /// <summary>
    /// Creates a new user account based on the provided DTO.
    /// </summary>
    /// <param name="createUserAccountDto">The DTO containing user account details.</param>
    /// <returns>The created user account model.</returns>
    public async Task<UserAccountModel> CreateUserAsync(CreateUserAccountDto createUserAccountDto)
    {
        var userAccountEntity = _mapper.Map<UserAccountEntity>(createUserAccountDto);
        
        await CreateAsync(userAccountEntity);
        
        var userAccountModel = _mapper.Map<UserAccountModel>(userAccountEntity);
        return userAccountModel;
    }

    /// <summary>
    /// Adds the specified user to the provided roles.
    /// </summary>
    /// <param name="user">The user account to add roles to.</param>
    /// <param name="roles">The roles to assign to the user.</param>
    public async Task AddUserToRolesAsync(UserAccountAggregate user, IEnumerable<string> roles)
    {
        await _userManager.AddToRolesAsync(user, roles);
    }
}