using AutoMapper;
using taf_server.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using taf_server.Domain.Aggregates;
using taf_server.Domain.Interfaces;
using taf_server.Domain.Model;
using taf_server.Infrastructure.Entities;
using taf_server.Presentations.Dtos.Authentication;
using taf_server.Presentations.Dtos.UserAccount;

namespace taf_server.Infrastructure.Repositories;

public class UserAccountCommandRepository : IUserAccountCommandRepository
{
    private readonly IMapper _mapper;
    private readonly UserManager<UserAccountAggregate> _userManager;

    public UserAccountCommandRepository(
        IMapper mapper,
        UserManager<UserAccountAggregate> userManager)
    {
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<UserAccountAggregate?> FindByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<UserAccountAggregate?> FindByPhoneNumberAsync(string phoneNumber)
    {
        return await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
    }

    public async Task<UserAccountModel> CreateUserAsync(CreateUserAccountDto createUserAccountDto)
    {
        var userAccountEntity = _mapper.Map<UserAccountEntity>(createUserAccountDto);
        
        var userAccountModel = _mapper.Map<UserAccountModel>(userAccountEntity);
        return userAccountModel;
    }

    public async Task AddUserToRolesAsync(UserAccountAggregate user, IEnumerable<string> roles)
    {
        await _userManager.AddToRolesAsync(user, roles);
    }
}