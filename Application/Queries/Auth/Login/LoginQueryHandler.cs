using System.Security.Authentication;
using Application.Exceptions;
using Domain.Interfaces;
using Domain.Model;
using Domain.SeedWork.Enums.UserAccount;
using Domain.SeedWork.Query;

namespace Application.Queries.Auth.Login;

public class LoginQueryHandler : IQueryHandler<LoginQuery, TokenModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtTokenService;

    public LoginQueryHandler(
        IUnitOfWork unitOfWork,
        IJwtService jwtTokenService
        )
    {
        _unitOfWork = unitOfWork;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<TokenModel> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var userLoginDataModel = await _unitOfWork.UserLoginDataQueryRepository.FindOneByEmail(request.Email);
        
        if (userLoginDataModel == null)
            throw new InvalidCredentialException("Invalid credentials");
        
        bool isPasswordMatch = await _unitOfWork.UserLoginDataQueryRepository.IsPasswordMatch(request.Email, request.Password);
        
        if (!isPasswordMatch)
            throw new InvalidCredentialException("Invalid credentials");
        request.Password = null;
        
        if (userLoginDataModel.UserAccount.Status == UserAccountStatus.Blocked)
            throw new BadRequestException();
        
        if (userLoginDataModel.UserAccount.Status == UserAccountStatus.Inactive)
            throw new BadRequestException();

        return await _jwtTokenService.ResponseAuthWithAccessTokenAndRefreshTokenCookie(userLoginDataModel);
    }
}