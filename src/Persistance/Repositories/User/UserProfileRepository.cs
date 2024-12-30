using Domain.Aggregates;
using Domain.Interfaces.User;
using Persistance.Data;

namespace Persistance.Repositories.User;

/// <summary>
/// Repository for managing user profiles, including profile creation and status retrieval.
/// </summary>
public class UserProfileRepository
    : RepositoryBase<UserProfileAggregate>, IUserProfileRepository
{

    /// <summary>
    /// Initializes a new instance of the <see cref="UserProfileRepository"/> class.
    /// </summary>
    /// <param name="context">The application database context for accessing the database.</param>
    /// <param name="mapper">The AutoMapper instance for mapping between models and entities.</param>
    public UserProfileRepository(
        ApplicationDbContext context
        )
        : base(context)
    {
    }
    
    /// <summary>
    /// Creates a new user profile asynchronously.
    /// </summary>
    /// <param name="request">The user profile model containing the details of the profile to be created.</param>
    /// <returns>A result containing the created user profile model or a failure message.</returns>
    public async Task<UserProfileAggregate?> CreateUserProfileAsync(UserProfileAggregate userProfileAggregate)
    {
        var created = await CreateAsync(userProfileAggregate);
        
        return created?.Entity;
    }

    public async Task<UserProfileAggregate?> UpdateUserProfileAsync(UserProfileAggregate userProfileAggregate)
    {
        return await UpdateAsync(userProfileAggregate);
    }
    
    /// <summary>
    /// Retrieves the status of a user account asynchronously by user ID.
    /// </summary>
    /// <param name="userId">The user ID for which the account status is to be retrieved.</param>
    /// <returns>The status of the user account.</returns>
    public async Task<UserProfileAggregate?> GetUserProfileAsync(int eid)
    {
       return await GetByIdAsync(eid);
    }
    
    public async Task<UserProfileAggregate?> SoftDeleteUserAccount(UserProfileAggregate userProfileAggregate)
    {
        return await UpdateAsync(userProfileAggregate);
    }
}