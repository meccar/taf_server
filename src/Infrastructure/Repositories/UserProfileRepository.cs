using AutoMapper;
using DataBase.Data;
using Domain.Aggregates;
using Domain.Interfaces;
using Domain.SeedWork.Results;
using Shared.Model;

namespace Infrastructure.Repositories;

public class UserProfileRepository
    : RepositoryBase<UserProfileAggregate>, IUserProfileRepository
{
    private readonly IMapper _mapper;

    public UserProfileRepository(
        ApplicationDbContext context,
        IMapper mapper)

        : base(context)
    {
        _mapper = mapper;

    }
    public async Task<UserAccountResult> CreateUserAccountAsync(UserAccountModel request)
    {
        var userAccountEntity = _mapper.Map<UserProfileAggregate>(request);
        
        var created = await CreateAsync(userAccountEntity);
        
        if(!created)
            return UserAccountResult.Failure("Failed to create user account. Please try again.");
    
        var userAccountModel = _mapper.Map<UserAccountModel>(userAccountEntity);
        
        return UserAccountResult.Success(userAccountModel);
    }
    public async Task<string> GetUserAccountStatusAsync(string userId)
    {
        var userAccount = await GetByIdAsync(userId);
        return userAccount.Status;
    }
}