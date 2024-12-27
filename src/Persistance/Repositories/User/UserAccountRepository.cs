using Domain.Aggregates;
using Domain.Interfaces.Credentials;
using Domain.Interfaces.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.FileObjects;
using Shared.Model;
using Shared.Results;

namespace Persistance.Repositories.User;

/// <summary>
/// Repository for managing user accounts, including creation, validation, and login data checks.
/// </summary>
public class UserAccountRepository
    : IUserAccountRepository
{
    private readonly UserManager<UserAccountAggregate> _userManager;
    private readonly IMfaRepository _mfaRepository;
    private readonly IMailRepository _mailRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserAccountRepository"/> class.
    /// </summary>
    /// <param name="userManager">The UserManager for managing user accounts.</param>
    /// <param name="mfaRepository">The repository responsible for MFA setup.</param>
    /// <param name="mailRepository">The repository responsible for sending confirmation emails.</param>
    public UserAccountRepository(
        UserManager<UserAccountAggregate> userManager,
        IMfaRepository mfaRepository,
        IMailRepository mailRepository,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _userManager = userManager;
        _mfaRepository = mfaRepository;
        _mailRepository = mailRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IdentityResult> CreateAsync(UserAccountAggregate userAccountAggregate, string password)
    {
        userAccountAggregate.UserName ??= userAccountAggregate.Email;
        return await _userManager.CreateAsync(userAccountAggregate, password);
    }
    
    public async Task<IdentityResult> AddToRoleAsync(UserAccountAggregate userAccountAggregate, string role)
    {
        return await _userManager.AddToRoleAsync(userAccountAggregate, role);
    }
    
    /// <summary>
    /// Creates a new user account asynchronously.
    /// </summary>
    /// <param name="request">The user account model containing the details of the account to be created.</param>
    /// <returns>A result containing the created user account model or a failure message.</returns>
    // public async Task<Result<UserAccountAggregate>> CreateUserAccountAsync(UserAccountAggregate userAccountAggregate, string password)
    // {
    //     userAccountAggregate.UserName ??= userAccountAggregate.Email;
    //     
    //     IdentityResult result = await _userManager.CreateAsync(userAccountAggregate, password);
    //     password = null!;
    //     
    //     if (!result.Succeeded)
    //         return Result<UserAccountAggregate>.Failure(
    //             result.Errors.Select(e => e.Description).ToArray());
    //     
    //     IdentityResult roleResult = await _userManager.AddToRoleAsync(userAccountAggregate, FoRole.User);
    //     if (!roleResult.Succeeded)
    //         return Result<UserAccountAggregate>.Failure(
    //             roleResult.Errors.Select(e => e.Description).ToArray());
    //     
    //     Result<MfaViewModel> mfaViewModel = await _mfaRepository.MfaSetup(userAccountAggregate);
    //     if (!mfaViewModel.Succeeded)
    //         return Result<UserAccountAggregate>.Failure(
    //             roleResult.Errors.Select(e => e.Description).ToArray());
    //     
    //     Result isMailSent =  await _mailRepository.SendEmailConfirmation(userAccountAggregate, mfaViewModel.Value!);
    //     
    //     return isMailSent.Succeeded
    //         ? Result<UserAccountAggregate>.Success(userAccountAggregate)
    //         : Result<UserAccountAggregate>.Failure(
    //             result.Errors.Select(e => e.Description).ToArray());
    // }

    public async Task<Result<UserAccountAggregate>> UpdateUserAccountAsync(UserAccountAggregate userAccountAggregate)
    {
        var result = await _userManager.UpdateAsync(userAccountAggregate);
        
        return result.Succeeded
            ? Result<UserAccountAggregate>.Success(userAccountAggregate)
            : Result<UserAccountAggregate>.Failure(result.Errors.FirstOrDefault()?.Description ?? "An unknown error occurred");
    }
    
    /// <summary>
    /// Checks whether a user with the same login data (email or phone number) already exists.
    /// </summary>
    /// <param name="userLoginDataModel">The user account model to check for existing login data.</param>
    /// <returns>True if the login data already exists; otherwise, false.</returns>
    public async Task<bool> IsUserLoginDataExisted(UserAccountAggregate userAccountAggregate)
    {
        bool email = await _userManager.Users
            .AsQueryable()
            .AnyAsync(
                u =>
                    u.Email == userAccountAggregate.Email);
        
        bool phone = await _userManager.Users
            .AsQueryable()
            .AnyAsync(
                u =>
                    u.PhoneNumber == userAccountAggregate.PhoneNumber);
        
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

    public async Task<Result<UserAccountAggregate>> IsExistingAndVerifiedUserAccount(string Eid)
    {
        var result = await _userManager
                                            .Users
                                            .AsQueryable()
                                            .FirstOrDefaultAsync(
                                                u 
                                                    => u.EId == Eid);
        if (result == null)
            return Result<UserAccountAggregate>.Failure("Account does not exist");
        
        if (result.EmailConfirmed)
            return Result<UserAccountAggregate>.Failure("Account's email is already confirmed");
        
        return Result<UserAccountAggregate>.Success(result);
    }
    
    public async Task<Result<UserAccountAggregate>> GetCurrentUser()
    {
        var result = _httpContextAccessor.HttpContext?.User;
        return result != null 
            ? Result<UserAccountAggregate>.Success((await _userManager.GetUserAsync(result))!)
            : Result<UserAccountAggregate>.Failure("You do not have permission");
    }
    
    public async Task<Result<UserAccountAggregate>> GetCurrentUser(string eid)
    {
        var result = _httpContextAccessor.HttpContext?.User;
        
        if (result == null)
            return Result<UserAccountAggregate>.Failure("You do not have permission");
        
        var user = await _userManager.GetUserAsync(result);
        
        return user!.EId == eid 
            ? Result<UserAccountAggregate>.Success(user)
            : Result<UserAccountAggregate>.Failure("You do not have permission");
    }
}