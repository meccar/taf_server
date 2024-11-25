using AutoMapper;
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
    public async Task<bool> IsUserLoginDataExisted(string loginCredential)
    {
        var result = await _userManager.Users
            .AsNoTracking()
            .AnyAsync(u => u.Email == loginCredential || u.PhoneNumber == loginCredential);
        return result;
    }
    public async Task<bool> ValidateUserLoginData(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        
        if (user == null)
        {
            return false;
        }
        
        var validationResults = await Task.WhenAll(
            _userManager.IsEmailConfirmedAsync(user),
            _userManager.IsLockedOutAsync(user),
            _userManager.CheckPasswordAsync(user, password),
            _userManager.IsPhoneNumberConfirmedAsync(user)
        );
        
        var isValid = validationResults[0] && // isEmailConfirmed
                      !validationResults[1] && // !isLockedOut
                      validationResults[2] && // isPasswordValid
                      validationResults[3] && // isPhoneNumberConfirmed
                      user.IsTwoFactorEnabled && 
                      user.IsTwoFactorVerified;

        if (isValid)
            await _userManager.ResetAccessFailedCountAsync(user);
        
        return isValid;
    }
}