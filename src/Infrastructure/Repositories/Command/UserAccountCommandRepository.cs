using AutoMapper;
using Domain.Aggregates;
using Domain.Entities;
using Domain.Interfaces.Command;
using Domain.Model;
using Domain.SeedWork.Results;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories.Command;

/// <summary>
/// Represents the command repository for managing user account entities and operations.
/// </summary>
/// <remarks>
/// This class provides methods to interact with user account data, including creating new user accounts 
/// and checking for existing user account data. It leverages AutoMapper for object mapping and 
/// <see cref="UserManager{TUser}"/> for user management tasks.
/// </remarks>
public class UserAccountCommandRepository
    : RepositoryBase<UserAccountAggregate>, IUserAccountCommandRepository
{
    private readonly IMapper _mapper;
    private readonly UserManager<UserLoginDataEntity> _userManager;
    // private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserAccountCommandRepository"/> class.
    /// </summary>
    /// <param name="context">The database context used for data operations.</param>
    /// <param name="mapper">The AutoMapper instance for object mapping.</param>
    /// <param name="userManager">The UserManager for managing user accounts.</param>
    public UserAccountCommandRepository(
        ApplicationDbContext context,
        IMapper mapper,
        UserManager<UserLoginDataEntity> userManager
        ) : base(context)
    {
        // _context = context;
        _mapper = mapper;
        _userManager = userManager;
    }

    /// <summary>
    /// Creates a new user account based on the provided DTO.
    /// </summary>
    /// <param name="createUserAccountDto">The DTO containing user account details.</param>
    /// <returns>The created user account model.</returns>
    public async Task<UserAccountResult> CreateUserAccountAsync(UserAccountModel request)
    {
        var userAccountEntity = _mapper.Map<UserAccountAggregate>(request);
        
        var created = await CreateAsync(userAccountEntity);
        
        if(!created)
            return UserAccountResult.Failure("Failed to create user account. Please try again.");
    
        var userAccountModel = _mapper.Map<UserAccountModel>(userAccountEntity);
        
        return UserAccountResult.Success(userAccountModel);
    }

}