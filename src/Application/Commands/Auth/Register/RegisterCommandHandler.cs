using AutoMapper;
using Domain.Interfaces;
using Domain.SeedWork.Command;
using Shared.Dtos.Authentication.Register;
using Shared.Dtos.Exceptions;
using Shared.Model;

namespace Application.Commands.Auth.Register;

/// <summary>
/// Handles the execution of the <see cref="RegisterCommand"/> to register a new user.
/// This handler orchestrates the creation of both the user profile and user account, including validation of existing data.
/// </summary>
public class RegisterCommandHandler : TransactionalCommandHandler<RegisterCommand, RegisterUserResponseDto>
{
    private readonly IMapper _mapper;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work that coordinates the transaction scope for the command execution.</param>
    /// <param name="mapper">The AutoMapper instance used to map between different data models and DTOs.</param>
    public RegisterCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) : base(unitOfWork)
    {
        _mapper = mapper;
    }
    
    /// <summary>
    /// Executes the core logic for handling the registration of a user.
    /// It validates the user's login data, creates a new user profile, associates the profile with the user account,
    /// and returns a <see cref="RegisterUserResponseDto"/> containing the created user's information.
    /// </summary>
    /// <param name="request">The command request containing the user profile and user account data to be registered.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for asynchronous operations to complete.</param>
    /// <returns>A <see cref="Task{RegisterUserResponseDto}"/> representing the asynchronous operation, with the registered user's data as the result.</returns>
    /// <exception cref="BadRequestException">
    /// Thrown if the user's login data (email or phone number) already exists or if the creation of the user profile or account fails.
    /// </exception>
    protected override async Task<RegisterUserResponseDto> ExecuteCoreAsync(RegisterCommand request, CancellationToken cancellationToken)
    {
        var userProfileModel = _mapper.Map<UserProfileModel>(request.UserProfileModel);
        var userAccountModel = _mapper.Map<UserAccountModel>(request.UserAccountModel);
        
        // Validate existing user login data
        if (await UnitOfWork.UserAccountRepository.IsUserLoginDataExisted(userAccountModel))
            throw new BadRequestException("Either Email or Phone number already exists");

        // Create user account
        var userProfile = await UnitOfWork
            .UserProfileRepository
            .CreateUserProfileAsync(userProfileModel);

        if (!userProfile.Succeeded)
            throw new BadRequestException("Failed to create user account");

        // Associate login data with the new account
        userAccountModel.UserProfileId = userProfile.Value!.Id;

        var userAccount = await UnitOfWork
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
