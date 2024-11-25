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

    public async Task<string> GetUserAccountStatusAsync(string userId)
    {
        var userAccount = await GetByIdAsync(userId);
        return userAccount.Status;
    }
}