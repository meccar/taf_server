using Application.Exceptions;
using Application.Helper;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;
using Domain.SeedWork.Command;
using Infrastructure.Data;

namespace Application.Commands.Auth.Register;

/// <summary>
/// Handles the registration process for new user accounts.
/// </summary>
/// <remarks>
/// This command handler is responsible for validating user input during the registration process.
/// It checks for the existence of a user's email and phone number to prevent duplicate accounts.
/// If the provided email or phone number already exists in the system, a 
/// <see cref="BadRequestException"/> is thrown with an appropriate message. 
/// Upon successful validation, a new user account and associated login data are created.
/// </remarks>

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, UserAccountModel>
{
    private readonly IUnitOfWork _unitOfWork;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work instance to manage data transactions.</param>
    public RegisterCommandHandler(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    /// <summary>
    /// Handles the registration command by validating user data and creating a new account.
    /// </summary>
    /// <param name="request">The registration command containing user details.</param>
    /// <param name="cancellationToken">A cancellation token to signal cancellation of the operation.</param>
    /// <returns>A task that represents the asynchronous operation, containing the created <see cref="UserAccountModel"/>.</returns>
    /// <exception cref="BadRequestException">Thrown when the email or phone number is already in use.</exception>
    public async Task<UserAccountModel> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // using var transaction = await _context.Database.BeginTransactionAsync();
        
        if (await _unitOfWork.UserLoginDataQueryRepository.IsUserLoginDataExisted(request.UserLoginDataModel.Email))
            throw new BadRequestException("Email already exists");
        
        if (await _unitOfWork.UserLoginDataQueryRepository.IsUserLoginDataExisted(request.UserLoginDataModel.PhoneNumber))
            throw new BadRequestException("Phone number already exists");
        
        var userAccount = await _unitOfWork
            .UserAccountCommandRepository
            .CreateUserAccountAsync(request.UserAccountModel);
        request.UserLoginDataModel.UserAccountId = userAccount.Id;
        
        userAccount.UserLoginData = await _unitOfWork
            .UserLoginDataCommandRepository
            .CreateUserLoginDataAsync(request.UserLoginDataModel);
        
        return userAccount;
    }
}
