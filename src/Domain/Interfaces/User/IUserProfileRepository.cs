using Domain.Abstractions;
using Domain.Aggregates;

namespace Domain.Interfaces.User;

/// <summary>
/// Defines the contract for interacting with user profile data in the repository.
/// </summary>
/// <remarks>
/// This interface provides methods for creating user profiles and retrieving user account status. 
/// It acts as a contract for the repository layer handling the persistence of user profile information.
/// </remarks>
public interface IUserProfileRepository : IRepositoryBase<UserProfileAggregate>
{
}