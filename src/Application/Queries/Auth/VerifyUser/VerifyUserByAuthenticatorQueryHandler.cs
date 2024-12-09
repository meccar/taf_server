using AutoMapper;
using Domain.Interfaces;
using Domain.SeedWork.Query;
using Shared.Dtos.Authentication.Credentials;
using Shared.Dtos.Exceptions;
using Shared.Results;

namespace Application.Queries.Auth.VerifyUser;

public class VerifyUserByAuthenticatorQueryHandler : IQueryHandler<VerifyUserByAuthenticatorQuery, VerifyUserResponseDto>
{
    private readonly IMfaRepository _mfaRepository;
    private readonly IMailRepository _mailRepository;
    private readonly IJwtRepository _jwtTokenRepository;
    private readonly IMapper _mapper;
    
    public VerifyUserByAuthenticatorQueryHandler(
        IMfaRepository mfaRepository,
        IMailRepository mailRepository,
        IJwtRepository jwtTokenRepository,
        IMapper mapper
    )
    {
        _mfaRepository = mfaRepository;
        _mailRepository = mailRepository;
        _jwtTokenRepository = jwtTokenRepository;
        _mapper = mapper;
    }
    
    public async Task<VerifyUserResponseDto> Handle(VerifyUserByAuthenticatorQuery request, CancellationToken cancellationToken)
    {
        string? verifyEmailConfirmationTokenResult = await _mailRepository.VerifyEmailConfirmationToken(request.UrlToken);

        if (verifyEmailConfirmationTokenResult == null)
        {
            throw new BadRequestException("Bad request");
        }
        
        Result validateMfaResult = await _mfaRepository.ValidateMfa(verifyEmailConfirmationTokenResult, request.AuthenticatorToken);

        if (!validateMfaResult.Succeeded)
        {
            throw new BadRequestException("Bad request");
        }
        
        var tokenModel = await _jwtTokenRepository.GenerateAuthResponseWithRefreshTokenCookie(verifyEmailConfirmationTokenResult);
        return _mapper.Map<VerifyUserResponseDto>(tokenModel);
        
    }
}