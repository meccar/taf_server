using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using Shared.Model;
using Shared.Results;

namespace Infrastructure.Repositories;

public class UserAccountRepository
    : IUserAccountRepository
{
    private readonly IMapper _mapper;
    private readonly UserManager<UserAccountAggregate> _userManager;
    private readonly IMfaRepository _mfaRepository;
    private readonly IMailRepository _mailRepository;

    public UserAccountRepository(
        // ApplicationDbContext context,
        IMapper mapper,
        UserManager<UserAccountAggregate> userManager,
        IMfaRepository mfaRepository,
        IMailRepository mailRepository
            
        // RoleManager<IdentityRole> roleManager
    )
    {
        _mapper = mapper;
        _userManager = userManager;
        _mfaRepository = mfaRepository;
        _mailRepository = mailRepository;
        // _roleManager = roleManager;
    }
    
    public async Task<Result<UserAccountModel>> CreateUserAccountAsync(UserAccountModel request)
    {
        // var (userAccountAggregate, createResult) = await CreateUserAccountAsync(request);
        var userAccountAggregate = _mapper.Map<UserAccountAggregate>(request);
        userAccountAggregate.UserName ??= userAccountAggregate.Email;

        var result = await _userManager.CreateAsync(userAccountAggregate, request.Password);
        request.Password = null;
        
        if (!result.Succeeded)
        {
            return Result<UserAccountModel>.Failure(
                result.Errors.Select(e => e.Description).ToArray());
        }
        
        var roleResult = await AssignRoleAsync(userAccountAggregate);
        if (!roleResult.Succeeded)
        {
            return Result<UserAccountModel>.Failure(
                roleResult.Errors.Select(e => e.Description).ToArray());
        }
        
        await _mailRepository.SendEmailConfirmation(userAccountAggregate);
        bool isMfaSent = await _mfaRepository.MfaSetup(userAccountAggregate);
        
        if (isMfaSent)
        {
            var userLoginDataModel = _mapper.Map<UserAccountModel>(userAccountAggregate);
            return Result<UserAccountModel>.Success(userLoginDataModel);
        }
        
        return Result<UserAccountModel>.Failure(
            result.Errors.Select(e => e.Description).ToArray());
    }
    
    public async Task<bool> IsUserLoginDataExisted(UserAccountModel userLoginDataModel)
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
        
        return email || email;
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
    
    private async Task<IdentityResult> AssignRoleAsync(UserAccountAggregate user)
    {
        return await _userManager.AddToRoleAsync(user, FORole.User);
    }
}