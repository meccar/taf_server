using AutoMapper;
using Domain.Aggregates;
using Domain.Interfaces.Query;
using Domain.Model;
using Infrastructure.Data;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Query;

public class UserAccountQueryRepository
    : RepositoryBase<UserAccountEntity>, IUserAccountQueryRepository
{
    private readonly IMapper _mapper;
    private readonly UserManager<UserAccountAggregate> _userManager;
    
    public UserAccountQueryRepository(
        ApplicationDbContext context,
        IMapper mapper,
        UserManager<UserAccountAggregate> userManager)
        : base(context)
    {
        _mapper = mapper;
        _userManager = userManager;
    }
    
    /// <summary>
    /// Checks if a user account with the specified data exists.
    /// </summary>
    /// <param name="userAccountData">The user account data to check, such as a phone number.</param>
    /// <returns><c>true</c> if the user account exists; otherwise, <c>false</c>.</returns>
    public async Task<bool> IsUserAccountDataExisted(string userAccountData)
    {
        return await ExistAsync(u => u.PhoneNumber == userAccountData);
    }
    
    
    public async Task<UserAccountModel> FindOneByEmail(string email)
    {
        var query = FindByCondition(u => u.UserLoginData!.Email == email);
        var results = await query.ToListAsync();
        var userAccountModel = _mapper.Map<UserAccountModel>(results.Single());
        return userAccountModel;
    }
}