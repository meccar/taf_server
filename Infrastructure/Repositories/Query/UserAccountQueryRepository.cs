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
    : RepositoryBase<UserAccountAggregate>, IUserAccountQueryRepository
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context; 
        
    public UserAccountQueryRepository(
        ApplicationDbContext context,
        IMapper mapper)
        : base(context)
    {
        _context = context;
        _mapper = mapper;
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
}