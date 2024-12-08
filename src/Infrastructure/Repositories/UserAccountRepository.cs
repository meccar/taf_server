// using AutoMapper;
// using Domain.Aggregates;
// using Domain.Interfaces;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;
// using Shared.FileObjects;
// using Shared.Model;
// using Shared.Results;
//
// namespace Infrastructure.Repositories;
//
// /// <summary>
// /// Repository for managing user accounts, including creation, validation, and login data checks.
// /// </summary>
// public class UserAccountRepository
//     : IUserAccountRepository
// {
//     private readonly IMapper _mapper;
//     private readonly UserManager<UserAccountAggregate> _userManager;
//     private readonly IMfaRepository _mfaRepository;
//     private readonly IMailRepository _mailRepository;
//
//     /// <summary>
//     /// Initializes a new instance of the <see cref="UserAccountRepository"/> class.
//     /// </summary>
//     /// <param name="mapper">The AutoMapper instance for mapping models.</param>
//     /// <param name="userManager">The UserManager for managing user accounts.</param>
//     /// <param name="mfaRepository">The repository responsible for MFA setup.</param>
//     /// <param name="mailRepository">The repository responsible for sending confirmation emails.</param>
//     public UserAccountRepository(
//         IMapper mapper,
//         UserManager<UserAccountAggregate> userManager,
//         IMfaRepository mfaRepository,
//         IMailRepository mailRepository
//             
//     )
//     {
//         _mapper = mapper;
//         _userManager = userManager;
//         _mfaRepository = mfaRepository;
//         _mailRepository = mailRepository;
//     }
//     
//     /// <summary>
//     /// Creates a new user account asynchronously.
//     /// </summary>
//     /// <param name="request">The user account model containing the details of the account to be created.</param>
//     /// <returns>A result containing the created user account model or a failure message.</returns>
//     public async Task<Result<UserAccountModel>> CreateUserAccountAsync(UserAccountModel request)
//     {
//         UserAccountAggregate userAccountAggregate = _mapper.Map<UserAccountAggregate>(request);
//         userAccountAggregate.UserName ??= userAccountAggregate.Email;
//
//         IdentityResult result = await _userManager.CreateAsync(userAccountAggregate, request.Password);
//         request.Password = null!;
//         
//         if (!result.Succeeded)
//         {
//             return Result<UserAccountModel>.Failure(
//                 result.Errors.Select(e => e.Description).ToArray());
//         }
//         
//         IdentityResult roleResult = await AssignRoleAsync(userAccountAggregate);
//         if (!roleResult.Succeeded)
//         {
//             return Result<UserAccountModel>.Failure(
//                 roleResult.Errors.Select(e => e.Description).ToArray());
//         }
//         
//         MfaViewModel mfaViewModel = await _mfaRepository.MfaSetup(userAccountAggregate);
//         Result isMailSent =  await _mailRepository.SendEmailConfirmation(userAccountAggregate, mfaViewModel);
//         
//         if (isMailSent.Succeeded)
//         {
//             UserAccountModel userLoginDataModel = _mapper.Map<UserAccountModel>(userAccountAggregate);
//             return Result<UserAccountModel>.Success(userLoginDataModel);
//         }
//         
//         return Result<UserAccountModel>.Failure(
//             result.Errors.Select(e => e.Description).ToArray());
//     }
//     
//     /// <summary>
//     /// Checks whether a user with the same login data (email or phone number) already exists.
//     /// </summary>
//     /// <param name="userLoginDataModel">The user account model to check for existing login data.</param>
//     /// <returns>True if the login data already exists; otherwise, false.</returns>
//     public async Task<bool> IsUserLoginDataExisted(UserAccountModel userLoginDataModel)
//     {
//         bool email = await _userManager.Users
//             .AsQueryable()
//             .AnyAsync(
//                 u =>
//                     u.Email == userLoginDataModel.Email);
//         
//         bool phone = await _userManager.Users
//             .AsQueryable()
//             .AnyAsync(
//                 u =>
//                     u.PhoneNumber == userLoginDataModel.PhoneNumber);
//         
//         return email || phone;
//     }
//     
//     /// <summary>
//     /// Validates the user's login data (email and password).
//     /// </summary>
//     /// <param name="email">The user's email.</param>
//     /// <param name="password">The user's password.</param>
//     /// <returns>True if the login data is valid; otherwise, false.</returns>
//     public async Task<bool> ValidateUserLoginData(string email, string password)
//     {
//         UserAccountAggregate? user = await _userManager.FindByEmailAsync(email);
//         
//         if (user == null)
//         {
//             return false;
//         }
//         
//         bool[] validationResults = await Task.WhenAll(
//             _userManager.IsEmailConfirmedAsync(user),
//             _userManager.IsLockedOutAsync(user),
//             _userManager.CheckPasswordAsync(user, password),
//             _userManager.IsPhoneNumberConfirmedAsync(user)
//         );
//         
//         bool isValid = validationResults[0] && // isEmailConfirmed
//                       !validationResults[1] && // !isLockedOut
//                       validationResults[2] && // isPasswordValid
//                       validationResults[3] && // isPhoneNumberConfirmed
//                       user.IsTwoFactorEnabled && 
//                       user.IsTwoFactorVerified;
//
//         if (isValid)
//             await _userManager.ResetAccessFailedCountAsync(user);
//         
//         return isValid;
//     }
//     
//     private async Task<IdentityResult> AssignRoleAsync(UserAccountAggregate user)
//     {
//         return await _userManager.AddToRoleAsync(user, FoRole.User);
//     }
// }