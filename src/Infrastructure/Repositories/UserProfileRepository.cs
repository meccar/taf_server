using AutoMapper;
using Domain.Aggregates;
using Domain.Interfaces;
using Persistance.Data;
using Shared.Model;
using Shared.Results;

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
    public async Task<Result<UserProfileModel>> CreateUserProfileAsync(UserProfileModel request)
    {
        var userProfileEntity = _mapper.Map<UserProfileAggregate>(request);
        
        var created = await CreateAsync(userProfileEntity);
        
        if(!created)
            return Result<UserProfileModel>.Failure("Failed to create user account. Please try again.");
    
        var userProfileModel = _mapper.Map<UserProfileModel>(userProfileEntity);
        
        return Result<UserProfileModel>.Success(userProfileModel);
    }
    public async Task<string> GetUserAccountStatusAsync(string userId)
    {
        var userAccount = await GetByIdAsync(userId);
        return userAccount.Status;
    }
}