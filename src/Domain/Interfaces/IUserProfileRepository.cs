using Shared.Model;
using Shared.Results;

namespace Domain.Interfaces;

public interface IUserProfileRepository
{
    Task<Result<UserProfileModel>> CreateUserProfileAsync(UserProfileModel createUserAccountDto);
    Task<string> GetUserAccountStatusAsync(string userId);

}