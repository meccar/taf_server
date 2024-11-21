using Application.Exceptions;
using Domain.Interfaces;
using Domain.Interfaces.Service;
using Domain.Model;
using Domain.SeedWork.Query;
using Duende.IdentityServer.Services;

namespace Application.Queries.Auth.Login;

public class LoginQueryHandler : IQueryHandler<LoginQuery, TokenModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtTokenService;
    private readonly IEventService _eventService;
    
    public LoginQueryHandler(
        IUnitOfWork unitOfWork,
        IJwtService jwtTokenService,
        IEventService eventService
        )
    {
        _unitOfWork = unitOfWork;
        _jwtTokenService = jwtTokenService;
        _eventService = eventService;
    }

    public async Task<TokenModel> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var userLoginDataModel = await _unitOfWork.UserLoginDataQueryRepository.FindOneByEmail(request.Email);
        
        if (userLoginDataModel == null)
            throw new UnauthorizedException("Invalid credentials");
        
        bool isPasswordMatch = await _unitOfWork.UserLoginDataQueryRepository.IsPasswordMatch(request.Email, request.Password);
        request.Password = null;
        
        if (!isPasswordMatch)
            throw new UnauthorizedException("Invalid credentials");
        
        // await _eventService.RaiseAsync(new LoginSuccessEvent(
        //     userLoginDataModel.EId,
        //     userLoginDataModel.Email
        // ));
        
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