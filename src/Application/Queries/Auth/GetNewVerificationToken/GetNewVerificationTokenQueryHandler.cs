using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.Credentials;
using Shared.Dtos;
using Shared.Dtos.Exceptions;

namespace Application.Queries.Auth.GetNewVerificationToken;

public class GetNewVerificationTokenQueryHandler 
    : TransactionalQueryHandler<GetNewVerificationTokenQuery, SuccessResponseDto>
{
    private readonly IMapper _mapper;
    private readonly IMfaRepository _mfaRepository;
    private readonly IMailRepository _mailRepository;

    public GetNewVerificationTokenQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMfaRepository mfaRepository,
        IMailRepository mailRepository
    ) : base(unitOfWork)
    {
        _mapper = mapper;
        _mfaRepository = mfaRepository;
        _mailRepository = mailRepository;
    }

    protected override async Task<SuccessResponseDto> ExecuteCoreAsync(
        GetNewVerificationTokenQuery request,
        CancellationToken cancellationToken)
    {
        var getRequestedUser = await UnitOfWork
            .UserAccountRepository
            .IsExistingAndVerifiedUserAccount(request.UserAccountEid);

        if (getRequestedUser == null)
            throw new BadRequestException("User not found");
        
        var mfaSetupResult = await _mfaRepository.MfaSetup(getRequestedUser);
        if (!mfaSetupResult.Succeeded)
            throw new BadRequestException(mfaSetupResult.Errors.FirstOrDefault()!);
        
        var isMailSent =  await _mailRepository.SendEmailConfirmation(getRequestedUser, mfaSetupResult.Value!);

        return isMailSent.Succeeded
            ? new SuccessResponseDto(true)
            : throw new BadRequestException(isMailSent.Errors.FirstOrDefault()!);
    }
}