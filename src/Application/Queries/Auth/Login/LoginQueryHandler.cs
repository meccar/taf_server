using Domain.Interfaces;
using Domain.SeedWork.Query;
using Shared.Dtos.Exceptions;
using Shared.Model;

namespace Application.Queries.Auth.Login;

public class LoginQueryHandler : IQueryHandler<LoginQuery, TokenModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtRepository _jwtTokenRepository;
    
    public LoginQueryHandler(
        IUnitOfWork unitOfWork,
        IJwtRepository jwtTokenRepository
        )
    {
        _unitOfWork = unitOfWork;
        _jwtTokenRepository = jwtTokenRepository;
    }

    public async Task<TokenModel> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.UserAccountRepository.ValidateUserLoginData(request.Email, request.Password))
            throw new UnauthorizedException("Invalid credentials");
        request.Password = null;

        return await _jwtTokenRepository.GenerateAuthResponseWithRefreshTokenCookie(request.Email);
    }
}