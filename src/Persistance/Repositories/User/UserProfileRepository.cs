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
}