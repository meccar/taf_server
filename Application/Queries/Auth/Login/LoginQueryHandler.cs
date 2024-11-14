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
            throw new UnauthorizedException("Invalid credentials");
        
        bool isPasswordMatch = await _unitOfWork.UserLoginDataQueryRepository.IsPasswordMatch(request.Email, request.Password);
        
        if (!isPasswordMatch)
            throw new UnauthorizedException("Invalid credentials");
        request.Password = null;
        
        // var userAcconutStatus = await _unitOfWork.UserAccountQueryRepository.GetUserAccountStatusAsync(userLoginDataModel.UserAccountId); 
        //
        // if (userAcconutStatus == UserAccountStatus.Blocked.ToString())
        //     throw new BadRequestException();
        //
        // if (userAcconutStatus == UserAccountStatus.Inactive.ToString())
        //     throw new BadRequestException();

        return await _jwtTokenService.GenerateAuthResponseWithRefreshTokenCookie(userLoginDataModel.Id);
    }
}