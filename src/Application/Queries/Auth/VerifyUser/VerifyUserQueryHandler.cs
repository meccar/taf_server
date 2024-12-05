using System.Text;
using AutoMapper;
using Domain.Interfaces;
using Domain.SeedWork.Query;
using Microsoft.AspNetCore.WebUtilities;
using Shared.Dtos.Authentication.Credentials;
using Shared.Dtos.Exceptions;
using Shared.Model;

namespace Application.Queries.Auth.VerifyUser;

public class VerifyUserQueryHandler : IQueryHandler<VerifyUserQuery, VerifyUserEmailRequestDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMailRepository _mailRepository;
    private readonly IJwtRepository _jwtTokenRepository;
    private readonly IMapper _mapper;

    public VerifyUserQueryHandler(
        IUnitOfWork unitOfWork,
        IMailRepository mailRepository,
        IJwtRepository jwtTokenRepository,
        IMapper mapper
    )
    {
        _unitOfWork = unitOfWork;
        _mailRepository = mailRepository;
        _jwtTokenRepository = jwtTokenRepository;
        _mapper = mapper;

    }
    public async Task<VerifyUserEmailRequestDto> Handle(VerifyUserQuery request, CancellationToken cancellationToken)
    {
        string? result = await _mailRepository.VerifyEmailConfirmationToken(request.Token.Token);

        if (result != null)
        {
            var tokenModel = await _jwtTokenRepository.GenerateAuthResponseWithRefreshTokenCookie(result);
            return _mapper.Map<VerifyUserEmailRequestDto>(tokenModel);
        }

        throw new BadRequestException("Bad request");
    }
}