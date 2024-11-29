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
    private readonly ApplicationDbContext _context;
    
    public UserLoginDataQueryRepository(
        ApplicationDbContext context,
        IMapper mapper,
        UserManager<UserLoginDataEntity> userManager)
    {
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
    }
    
    /// <summary>
    /// Checks if a user login data with the specified credential exists.
    /// </summary>
    /// <param name="userLoginData">The login credential to check, such as an email.</param>
    /// <returns><c>true</c> if the user login data exists; otherwise, <c>false</c>.</returns>
    public async Task<bool> IsUserLoginDataExisted(UserLoginDataModel userLoginDataModel)
    {
        var email = await _userManager.Users
            .AsQueryable()
            .AnyAsync(
                u =>
                    u.Email == userLoginDataModel.Email);
        
        var phone = await _context.Users
            .AsQueryable()
            .AnyAsync(
                u =>
                    u.PhoneNumber == userLoginDataModel.PhoneNumber);
        
        return email && email;
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