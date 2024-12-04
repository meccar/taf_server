using System.Text;
using Domain.Interfaces;
using Domain.SeedWork.Query;
using Microsoft.AspNetCore.WebUtilities;
using Shared.Dtos.Exceptions;
using Shared.Model;

namespace Application.Queries.Auth.VerifyUser;

public class VerifyUserQueryHandler : IQueryHandler<VerifyUserQuery, TokenModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMailRepository _mailRepository;
    private readonly IJwtRepository _jwtTokenRepository;

    public VerifyUserQueryHandler(
        IUnitOfWork unitOfWork,
        IMailRepository mailRepository,
        IJwtRepository jwtTokenRepository
    )
    {
        _unitOfWork = unitOfWork;
        _mailRepository = mailRepository;
        _jwtTokenRepository = jwtTokenRepository;
    }
    public async Task<TokenModel> Handle(VerifyUserQuery request, CancellationToken cancellationToken)
    {
        string? result = await _mailRepository.VerifyEmailConfirmationToken(request.Token);

        if (result != null)
            return await _jwtTokenRepository.GenerateAuthResponseWithRefreshTokenCookie(result);

        throw new BadRequestException("Bad request");
    }
}