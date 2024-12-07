﻿using AutoMapper;
using Domain.Interfaces;
using Domain.SeedWork.Command;
using Shared.Dtos.Authentication.Register;
using Shared.Dtos.Exceptions;
using Shared.Model;

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

public class RegisterCommandHandler : TransactionalCommandHandler<RegisterCommand, RegisterUserResponseDto>
{
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work instance to manage data transactions.</param>
    public RegisterCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the registration command by validating user data and creating a new account.
    /// </summary>
    /// <param name="request">The registration command containing user details.</param>
    /// <param name="cancellationToken">A cancellation token to signal cancellation of the operation.</param>
    /// <returns>A task that represents the asynchronous operation, containing the created <see cref="UserAccountModel"/>.</returns>
    /// <exception cref="BadRequestException">Thrown when the email or phone number is already in use.</exception>
    protected override async Task<RegisterUserResponseDto> ExecuteCoreAsync(RegisterCommand request, CancellationToken cancellationToken)
    {
        var userProfileModel = _mapper.Map<UserProfileModel>(request.UserProfileModel);
        var userAccountModel = _mapper.Map<UserAccountModel>(request.UserAccountModel);
        
        // Validate existing user login data
        if (await _unitOfWork.UserAccountRepository.IsUserLoginDataExisted(userAccountModel))
            throw new BadRequestException("Either Email or Phone number already exists");

        // Create user account
        var userProfile = await _unitOfWork
            .UserProfileRepository
            .CreateUserProfileAsync(userProfileModel);

        if (!userProfile.Succeeded)
            throw new BadRequestException("Failed to create user account");

        // Associate login data with the new account
        userAccountModel.UserProfileId = userProfile.Value.Id;

        var userAccount = await _unitOfWork
            .UserAccountRepository
            .CreateUserAccountAsync(userAccountModel);

        if (!userAccount.Succeeded)
            throw new BadRequestException("Failed to create user login data");

        // Attach login data to the user account
        userProfile.Value.UserAccount = userAccount.Value;

        var response = _mapper.Map<RegisterUserResponseDto>(userProfile.Value);

        return response;
    }
}
