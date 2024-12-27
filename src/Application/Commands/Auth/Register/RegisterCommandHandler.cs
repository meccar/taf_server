using AutoMapper;
using Domain.Aggregates;
using Domain.Interfaces;
using Domain.Interfaces.Credentials;
using Shared.Dtos.Authentication.Register;
using Shared.Dtos.Exceptions;
using Shared.FileObjects;

namespace Application.Commands.Auth.Register;

/// <summary>
/// Handles the execution of the <see cref="RegisterCommand"/> to register a new user.
/// This handler orchestrates the creation of both the user profile and user account, including validation of existing data.
/// </summary>
public class RegisterCommandHandler : TransactionalCommandHandler<RegisterCommand, RegisterUserResponseDto>
{
    private readonly IMapper _mapper;
    private readonly IMfaRepository _mfaRepository;
    private readonly IMailRepository _mailRepository;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work that coordinates the transaction scope for the command execution.</param>
    /// <param name="mapper">The AutoMapper instance used to map between different data models and DTOs.</param>
    public RegisterCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMfaRepository mfaRepository,
        IMailRepository mailRepository
        ) : base(unitOfWork)
    {
        _mapper = mapper;
        _mfaRepository = mfaRepository;
        _mailRepository = mailRepository;
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
        var userProfileAggregate = _mapper.Map<UserProfileAggregate>(request);
        var userAccountAggregate = _mapper.Map<UserAccountAggregate>(request);
        
        // Validate existing user login data
        if (await UnitOfWork.UserAccountRepository.IsUserLoginDataExisted(userAccountAggregate))
            throw new BadRequestException("Either Email or Phone number already exists");

        userProfileAggregate.IsDeleted = false;
        userProfileAggregate.DeletedAt = null;
        userProfileAggregate.CreatedAt = DateTime.Now;
        
        // Create user account
        var userProfile = await UnitOfWork
            .UserProfileRepository
            .CreateUserProfileAsync(userProfileAggregate);

        if (userProfile == null)
            throw new BadRequestException("Failed to create user account");

        // Associate login data with the new account
        userAccountAggregate.UserProfileId = userProfile.Id;

        // var userAccount = await UnitOfWork
        //     .UserAccountRepository
        //     .CreateUserAccountAsync(userAccountAggregate, request.Password);

        var createUserAccountResult = await UnitOfWork
            .UserAccountRepository
            .CreateAsync(userAccountAggregate, request.Password);
        
        if (!createUserAccountResult.Succeeded)
            throw new BadRequestException("Failed to create user account");

        var roleResult = await UnitOfWork
            .UserAccountRepository
            .AddToRoleAsync(userAccountAggregate, FoRole.User);
        
        if (!roleResult.Succeeded)
            throw new BadRequestException("Failed to create user account");
        
        var mfaResult = await _mfaRepository
            .MfaSetup(userAccountAggregate);
        
        if (!mfaResult.Succeeded)
            throw new BadRequestException("Failed to create user account");
        
        var mailResult = await _mailRepository
            .SendEmailConfirmation(userAccountAggregate, mfaResult.Value!);
        
        if (!mailResult.Succeeded)
            throw new BadRequestException("Failed to create user account");
        
        // Attach login data to the user account
        userProfile.UserAccount = userAccountAggregate!;

        var response = _mapper.Map<RegisterUserResponseDto>(userProfile);

        return response;
    }
}
