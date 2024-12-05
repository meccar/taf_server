using AutoMapper;
using Domain.Interfaces;
using Domain.SeedWork.Query;
using Shared.Dtos.Authentication.Login;
using Shared.Dtos.Exceptions;
using Shared.Model;

namespace Application.Queries.Auth.Login;

public class LoginQueryHandler : IQueryHandler<LoginQuery, LoginResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtRepository _jwtTokenRepository;
    private readonly IMapper _mapper;
    public LoginQueryHandler(
        IUnitOfWork unitOfWork,
        IJwtRepository jwtTokenRepository,
        IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _jwtTokenRepository = jwtTokenRepository;
        _mapper = mapper;
    }

    public async Task<LoginResponseDto> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.UserAccountRepository.ValidateUserLoginData(request.Email, request.Password))
            throw new UnauthorizedException("Invalid credentials");
        request.Password = null;

        var tokenModel = await _jwtTokenRepository.GenerateAuthResponseWithRefreshTokenCookie(request.Email);
        return _mapper.Map<LoginResponseDto>(tokenModel); 
    }
}