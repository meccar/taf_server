using AutoMapper;
using Domain.Aggregates;
using Domain.Entities;
using Domain.Interfaces.Query;
using Domain.Model;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Query;

public class UserAccountQueryRepository
    : IUserAccountQueryRepository
{
    private readonly IMapper _mapper;
    private readonly UserManager<UserAccountAggregate> _userManager;
    private readonly ApplicationDbContext _context; 
        
    public UserAccountQueryRepository(
        ApplicationDbContext context,
        IMapper mapper,
        UserManager<UserAccountAggregate> userManager)
    {
        _context = context;
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
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.PhoneNumber == userAccountData);
        return user != null;
    }
    
    
    public async Task<UserAccountModel> FindOneByEmail(string email)
    {
        var query = await _userManager.FindByEmailAsync(email);
        if (query == null)
        {
            return null;
        }
        var userAccountModel = _mapper.Map<UserAccountModel>(query);
        return userAccountModel;
    }
}