using System.Security.Authentication;
using Application.Exceptions;
using Domain.Interfaces;
using Domain.Model;
using Domain.SeedWork.Enums.UserAccount;
using Domain.SeedWork.Query;

namespace Application.Queries.Auth.Login;

public class LoginQueryHandler : IQueryHandler<LoginQuery, UserAccountModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public LoginQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserAccountModel> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var userAccountModel = await _unitOfWork.UserLoginDataQueryRepository.FindOneByEmail(request.Email);
        
        if (userAccountModel == null)
            throw new InvalidCredentialException("Invalid credentials");
        
        bool isPasswordMatch = await _unitOfWork.UserLoginDataQueryRepository.IsPasswordMatch(request.Email, request.Password);
        
        if (!isPasswordMatch)
            throw new InvalidCredentialException("Invalid credentials");
        request.Password = null;
        
        if (userAccountModel.UserAccount.Status == UserAccountStatus.Blocked)
            throw new BadRequestException();
        
        if (userAccountModel.UserAccount.Status == UserAccountStatus.Inactive)
            throw new BadRequestException();
        
        return null;
    }
}