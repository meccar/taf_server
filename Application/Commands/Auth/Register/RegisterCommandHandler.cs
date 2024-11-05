using Application.Exceptions;
using Application.Helper;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;
using Domain.SeedWork.Command;

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
    private readonly IMapper _mapper;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work instance to manage data transactions.</param>
    public RegisterCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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
        if (await _unitOfWork.UserLoginDataQueryRepository.IsUserLoginDataExisted(request.UserLogin.Email))
            throw new BadRequestException("Email already exists");

        if (await _unitOfWork.UserAccountQueryRepository.IsUserAccountDataExisted(request.UserAccount.PhoneNumber))
            throw new BadRequestException("Phone number already exists");

        var userAccountModel = _mapper.Map<UserAccountModel>(request.UserAccount);
        var userLoginDataModel = _mapper.Map<UserLoginDataModel>(request.UserLogin);
        
        // userLoginDataModel.PasswordHash = HashHelper.Encrypt(request.UserLogin.Password);
            
        var userAccount = await _unitOfWork.UserAccountCommandRepository.CreateUserAccountAsync(userAccountModel);
        userLoginDataModel.UserAccountId = userAccount.Id;
        
        userAccount.UserLoginData = await _unitOfWork.UserLoginDataCommandRepository.CreateUserLoginDataAsync(userLoginDataModel);
        
        // await _unitOfWork.CommitAsync();

        return userAccount;
    }
}
