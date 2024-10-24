using AutoMapper;
using Microsoft.AspNetCore.Identity;
using taf_server.Domain.Aggregates;
using taf_server.Domain.Interfaces.Query;
using taf_server.Domain.Repositories;
using taf_server.Infrastructure.Data;
using taf_server.Infrastructure.Entities;

namespace taf_server.Infrastructure.Repositories.Query;

public class UserLoginDataQueryRepository
    : RepositoryBase<UserLoginDataEntity>, IUserLoginDataQueryRepository
{
    private readonly IMapper _mapper;
    private readonly UserManager<UserAccountAggregate> _userManager;

    public UserLoginDataQueryRepository(
        ApplicationDbContext context,
        IMapper mapper,
        UserManager<UserAccountAggregate> userManager)
        : base(context)
    {
        _mapper = mapper;
        _userManager = userManager;
    }
    
    /// <summary>
    /// Checks if a user login data with the specified credential exists.
    /// </summary>
    /// <param name="userLoginData">The login credential to check, such as an email.</param>
    /// <returns><c>true</c> if the user login data exists; otherwise, <c>false</c>.</returns>
    public async Task<bool> IsUserLoginDataExisted(string userLoginData)
    {
        return await ExistAsync(u => u.Email == userLoginData);
    }

}