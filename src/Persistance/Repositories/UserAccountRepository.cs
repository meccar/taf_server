using AutoMapper;
using Domain.Aggregates;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.FileObjects;
using Shared.Model;
using Shared.Results;

namespace Persistance.Repositories;

/// <summary>
/// Repository for managing user accounts, including creation, validation, and login data checks.
/// </summary>
public class UserAccountRepository
    : IUserAccountRepository
{
    private readonly IMapper _mapper;
    private readonly UserManager<UserAccountAggregate> _userManager;
    private readonly IMfaRepository _mfaRepository;
    private readonly IMailRepository _mailRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserAccountRepository"/> class.
    /// </summary>
    /// <param name="mapper">The AutoMapper instance for mapping models.</param>
    /// <param name="userManager">The UserManager for managing user accounts.</param>
    /// <param name="mfaRepository">The repository responsible for MFA setup.</param>
    /// <param name="mailRepository">The repository responsible for sending confirmation emails.</param>
    public UserAccountRepository(
        IMapper mapper,
        UserManager<UserAccountAggregate> userManager,
        IMfaRepository mfaRepository,
        IMailRepository mailRepository
            
    )
    {
        _mapper = mapper;
        _userManager = userManager;
        _mfaRepository = mfaRepository;
        _mailRepository = mailRepository;
    }
    
    /// <summary>
    /// Creates a new user account asynchronously.
    /// </summary>
    /// <param name="request">The user account model containing the details of the account to be created.</param>
    /// <returns>A result containing the created user account model or a failure message.</returns>
    public async Task<Result<UserAccountModel>> CreateUserAccountAsync(UserAccountModel request)
    {
        UserAccountAggregate userAccountAggregate = _mapper.Map<UserAccountAggregate>(request);
        
        userAccountAggregate.UserName ??= userAccountAggregate.Email;
        // userAccountAggregate.TwoFactorEnabled = true;
        
        IdentityResult result = await _userManager.CreateAsync(userAccountAggregate, request.Password);
        request.Password = null!;
        
        if (!result.Succeeded)
            return Result<UserAccountModel>.Failure(
                result.Errors.Select(e => e.Description).ToArray());
        
        IdentityResult roleResult = await _userManager.AddToRoleAsync(userAccountAggregate, FoRole.User);
        if (!roleResult.Succeeded)
            return Result<UserAccountModel>.Failure(
                roleResult.Errors.Select(e => e.Description).ToArray());
        
        Result<MfaViewModel> mfaViewModel = await _mfaRepository.MfaSetup(userAccountAggregate);
        if (!mfaViewModel.Succeeded)
            return Result<UserAccountModel>.Failure(
                roleResult.Errors.Select(e => e.Description).ToArray());
        
        Result isMailSent =  await _mailRepository.SendEmailConfirmation(userAccountAggregate, mfaViewModel.Value!);
        
        if (isMailSent.Succeeded)
        {
            UserAccountModel userLoginDataModel = _mapper.Map<UserAccountModel>(userAccountAggregate);
            return Result<UserAccountModel>.Success(userLoginDataModel);
        }
        
        return Result<UserAccountModel>.Failure(
            result.Errors.Select(e => e.Description).ToArray());
    }
    
    /// <summary>
    /// Checks whether a user with the same login data (email or phone number) already exists.
    /// </summary>
    /// <param name="userLoginDataModel">The user account model to check for existing login data.</param>
    /// <returns>True if the login data already exists; otherwise, false.</returns>
    public async Task<bool> IsUserLoginDataExisted(UserAccountModel userLoginDataModel)
    {
        bool email = await _userManager.Users
            .AsQueryable()
            .AnyAsync(
                u =>
                    u.Email == userLoginDataModel.Email);
        
        bool phone = await _userManager.Users
            .AsQueryable()
            .AnyAsync(
                u =>
                    u.PhoneNumber == userLoginDataModel.PhoneNumber);
        
        return email || phone;
    }
    
    public async Task<bool> IsUserLoginDataExisted(string userLoginData)
    {
        bool result = await _userManager.Users
            .AsQueryable()
            .AnyAsync(
                u =>
                    u.Email == userLoginData || u.PhoneNumber == userLoginData);
        
        return result;
    }
    
    /// <summary>
    /// Validates the user's login data (email and password).
    /// </summary>
    /// <param name="email">The user's email.</param>
    /// <param name="password">The user's password.</param>
    /// <returns>True if the login data is valid; otherwise, false.</returns>
    public async Task<Result> ValidateUserLoginData(string email, string password)
    {
        UserAccountAggregate? user = await _userManager.FindByEmailAsync(email);
        
        if (user == null)
        {
            return Result.Failure("Invalid email or Password");
        }

        bool isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        bool isCorrectPassword = await _userManager.CheckPasswordAsync(user, password);
        
        if (isEmailConfirmed && isCorrectPassword && user.IsTwoFactorVerified)
        {
            await _userManager.ResetAccessFailedCountAsync(user);
            return Result.Success();

        }
        
        return Result.Failure("Invalid email or Password");
    }
}