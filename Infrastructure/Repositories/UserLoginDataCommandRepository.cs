using AutoMapper;
using Microsoft.AspNetCore.Identity;
using taf_server.Domain.Model;
using taf_server.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Presentations.Dtos.Authentication;
using taf_server.Domain.Aggregates;
using taf_server.Domain.Interfaces;
using taf_server.Infrastructure.Data;
using taf_server.Infrastructure.Entities;

namespace taf_server.Infrastructure.Repositories;
public class UserLoginDataCommandRepository 
    : RepositoryBase<UserLoginDataEntity>, IUserLoginDataCommandRepository
{
    private readonly IMapper _mapper;
    private readonly UserManager<UserLoginDataAggregate> _userManager;
    
    public UserLoginDataCommandRepository(
        ApplicationDbContext context,
        IMapper mapper,
        UserManager<UserLoginDataAggregate> userManager)
            : base(context)
    {
        _mapper = mapper;
        _userManager = userManager;
    }
    
    public async Task<bool> IsUserLoginDataExisted(string loginCredential)
    {
        return await ExistAsync(u => u.Email == loginCredential);
    }
}