using AutoMapper;
using Domain.Aggregates;
using Domain.Entities;
using Domain.Interfaces.Query;
using Domain.Model;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Query;

public class UserLoginDataQueryRepository
    : IUserLoginDataQueryRepository
{
    private readonly IMapper _mapper;
    private readonly UserManager<UserLoginDataEntity> _userManager;

    public UserLoginDataQueryRepository(
        ApplicationDbContext context,
        IMapper mapper,
        UserManager<UserLoginDataEntity> userManager)
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
        var user = await _userManager.FindByEmailAsync(userLoginData);
        return user != null;
    }
    public async Task<UserLoginDataModel> FindOneByEmail(string email)
    {
        var query = await _userManager.FindByEmailAsync(email);
        if (query == null)
        {
            return null;
        }
        var userLoginDataModel = _mapper.Map<UserLoginDataModel>(query);
        return userLoginDataModel;
    }
    
    public async Task<bool> IsUserAccountDataExisted(UserLoginDataModel userLoginDataModel)
    {
        var userLoginDataEntity = _mapper.Map<UserLoginDataEntity>(userLoginDataModel);

        var result = await _userManager.GetPhoneNumberAsync(userLoginDataEntity);
        return result != null;
    }
}