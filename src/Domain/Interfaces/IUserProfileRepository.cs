using Domain.SeedWork.Results;
using Shared.Model;

namespace Domain.Interfaces;

public interface IUserProfileRepository
{
    Task<UserAccountResult> CreateUserAccountAsync(UserProfileModel createUserAccountDto);
    Task<string> GetUserAccountStatusAsync(string userId);

}