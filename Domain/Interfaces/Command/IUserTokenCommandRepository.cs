using Domain.Model;

namespace Domain.Interfaces.Command;

public interface IUserTokenCommandRepository
{
    Task<UserTokenModel?> CreateUserTokenAsync(UserTokenModel request);
    Task<UserTokenModel> UpdateUserTokenAsync(UserTokenModel request);
    Task<List<UserTokenModel>?> TokenExistsAsync(string userId, UserTokenModel token);
    // Task<bool> TestSignInAsync(UserLoginDataModel user,  UserTokenModel token);
}