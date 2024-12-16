using AutoMapper;
using Domain.Aggregates;
using Domain.Interfaces.User;
using Microsoft.EntityFrameworkCore;
using Persistance.Data;
using Shared.Results;

namespace Persistance.Repositories.User;

/// <summary>
/// Repository for managing user profiles, including profile creation and status retrieval.
/// </summary>
public class UserProfileRepository
    : RepositoryBase<UserProfileAggregate>, IUserProfileRepository
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserProfileRepository"/> class.
    /// </summary>
    /// <param name="context">The application database context for accessing the database.</param>
    /// <param name="mapper">The AutoMapper instance for mapping between models and entities.</param>
    public UserProfileRepository(
        ApplicationDbContext context,
        IMapper mapper
        )
        : base(context)
    {
        _mapper = mapper;
        _context = context;
    }
    
    /// <summary>
    /// Creates a new user profile asynchronously.
    /// </summary>
    /// <param name="request">The user profile model containing the details of the profile to be created.</param>
    /// <returns>A result containing the created user profile model or a failure message.</returns>
    public async Task<Result<UserProfileAggregate>> CreateUserProfileAsync(UserProfileAggregate userProfileAggregate)
    {
        var created = await CreateAsync(userProfileAggregate);
        
        return created != null
            ? Result<UserProfileAggregate>.Success(created.Entity)
            : Result<UserProfileAggregate>.Failure("Failed to create user account. Please try again.");
    }

    public async Task<Result<UserProfileAggregate>> UpdateUserProfileAsync(UserProfileAggregate userProfileAggregate)
    {
        var updated = await UpdateAsync(userProfileAggregate);
        
        return updated != null
            ? Result<UserProfileAggregate>.Success(updated)
            : Result<UserProfileAggregate>.Failure("Failed to update user profile. Please try again.");
    }
    
    /// <summary>
    /// Retrieves the status of a user account asynchronously by user ID.
    /// </summary>
    /// <param name="userId">The user ID for which the account status is to be retrieved.</param>
    /// <returns>The status of the user account.</returns>
    public async Task<Result<UserProfileAggregate>> GetUserProfileAsync(int eid)
    {
        var userAccount = await GetByIdAsync(eid);
        
        return userAccount != null
            ? Result<UserProfileAggregate>.Success(userAccount)
            : Result<UserProfileAggregate>.Failure("Failed to get user profile. Please try again.");
    }
    
    public async Task<Result<UserProfileAggregate>> SoftDeleteUserAccount(UserProfileAggregate userProfileAggregate)
    {
        var updated = await UpdateAsync(userProfileAggregate);
        
        return updated != null
            ? Result<UserProfileAggregate>.Success(updated)
            : Result<UserProfileAggregate>.Failure("Failed to update user profile. Please try again.");
    }
}