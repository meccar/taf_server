using Domain.Aggregates;
using Shared.Model;

namespace Domain.Interfaces.Credentials;

/// <summary>
/// Defines the contract for handling sign-in operations, including user authentication.
/// </summary>
public interface ISignInRepository
{
    /// <summary>
    /// Authenticates a user with the provided email and password, and generates a result containing the 
    /// authenticated user's account and associated token model.
    /// </summary>
    /// <param name="email">The email address of the user attempting to sign in.</param>
    /// <param name="password">The password associated with the provided email address.</param>
    /// <param name="isPersistent">A boolean value indicating whether the authentication session should be persistent.</param>
    /// <returns>A <see cref="Result{T}"/> containing a tuple with the <see cref="UserAccountAggregate"/> 
    /// representing the authenticated user and the <see cref="UserTokenModel"/> for the session.</returns>
    Task<(UserAccountAggregate, UserTokenModel)?> SignInAsync(UserAccountAggregate user, string password, bool isPersistent);

}