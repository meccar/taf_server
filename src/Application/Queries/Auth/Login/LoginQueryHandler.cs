using Domain.Interfaces;
using Domain.Interfaces.Service;
using Domain.SeedWork.Query;
using Shared.Dtos.Exceptions;
using Shared.Model;

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
        if (!await _unitOfWork.UserAccountRepository.ValidateUserLoginData(request.Email, request.Password))
            throw new UnauthorizedException("Invalid credentials");
        request.Password = null;

        return await _jwtTokenService.GenerateAuthResponseWithRefreshTokenCookie(request.Email);
    }
}