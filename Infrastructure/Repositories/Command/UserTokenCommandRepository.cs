using AutoMapper;
using Azure.Core;
using Domain.Entities;
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
        IMapper mapper
        )
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<UserTokenModel?> CreateUserTokenAsync(UserTokenModel request)
    {
        var userTokenEntity = _mapper.Map<UserTokenEntity>(request);

        var query = await _context.UserTokens.AddAsync(userTokenEntity);


        if (query == null)
        {
            return null;
        }

        await _context.SaveChangesAsync();

        return _mapper.Map<UserTokenModel>(query);
    }
}