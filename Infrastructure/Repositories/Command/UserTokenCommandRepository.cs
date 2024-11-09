using AutoMapper;
using Domain.Interfaces.Command;
using Domain.Model;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Command;

public class UserTokenCommandRepository
    : IUserTokenCommandRepository
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public UserTokenCommandRepository(
    ApplicationDbContext context,
    IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<UserLoginDataModel?> CreateUserTokenAsync(UserTokenModel token)
    {
        var query = await _context.UserTokens.AddAsync(token);


        if (query == null)
        {
            return null;
        }

        await _context.SaveChangesAsync();

        return _mapper.Map<UserLoginDataModel>(query);
    }
}