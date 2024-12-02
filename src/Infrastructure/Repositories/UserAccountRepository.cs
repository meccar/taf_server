using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Service;
using Domain.SeedWork.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using Shared.Model;

namespace Infrastructure.Repositories;

public class UserAccountRepository
    : IUserAccountRepository
{
    private readonly IMapper _mapper;
    private readonly UserManager<UserAccountAggregate> _userManager;
    private readonly IMfaRepository _mfaRepository;

    public UserAccountRepository(
        // ApplicationDbContext context,
        IMapper mapper,
        UserManager<UserAccountAggregate> userManager,
        IMfaRepository mfaRepository

        // RoleManager<IdentityRole> roleManager
    )
    {
        _mapper = mapper;
        _userManager = userManager;
        _mfaRepository = mfaRepository;
        // _roleManager = roleManager;
    }
    
    public async Task<UserLoginDataResult> CreateUserLoginDataAsync(UserLoginDataModel request)
    {
        var (userAccountAggregate, createResult) = await CreateUserAccountAsync(request);
        if (!createResult.Succeeded)
        {
            return UserLoginDataResult.Failure(
                createResult.Errors.Select(e => e.Description).ToArray());
        }
        
        var roleResult = await AssignRoleAsync(userAccountAggregate);
        if (!roleResult.Succeeded)
        {
            return UserLoginDataResult.Failure(
                roleResult.Errors.Select(e => e.Description).ToArray());
        }

        var userLoginDataModel = _mapper.Map<UserLoginDataModel>(userAccountAggregate);
        return UserLoginDataResult.Success(userLoginDataModel);
        // if (await _mfaRepository.MfaSetup(userAccountAggregate))
        // {
        // }

        // return UserLoginDataResult.Failure(
        //     createResult.Errors.Select(e => e.Description).ToArray());
    }
    
    public async Task<bool> IsUserLoginDataExisted(UserLoginDataModel userLoginDataModel)
    {
        var email = await _userManager.Users
            .AsQueryable()
            .AnyAsync(
                u =>
                    u.Email == userLoginDataModel.Email);
        
        var phone = await _userManager.Users
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
    private async Task<(UserAccountAggregate User, IdentityResult Result)> CreateUserAccountAsync(
        UserLoginDataModel request)
    {
        var userEntity = _mapper.Map<UserAccountAggregate>(request);
        userEntity.UserName ??= userEntity.Email;

        var result = await _userManager.CreateAsync(userEntity, request.Password);
        request.Password = null;

        return (userEntity, result);
    }
    
    private async Task<IdentityResult> AssignRoleAsync(UserAccountAggregate user)
    {
        return await _userManager.AddToRoleAsync(user, FORole.User);
    }
}