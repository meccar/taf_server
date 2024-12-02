using AutoMapper;
using DataBase.Data;
using Domain.Aggregates;
using Domain.Interfaces.Query;

namespace Infrastructure.Repositories.Query;

public class UserAccountQueryRepository
    : RepositoryBase<UserProfileAggregate>, IUserAccountQueryRepository
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