using AutoMapper;
using Domain.Aggregates;
using Domain.Interfaces;
using Domain.SeedWork.Query;
using Shared.Dtos.Authentication.Credentials;
using Shared.Dtos.Exceptions;
using Shared.Results;

namespace Application.Queries.Auth.VerifyUser;

/// <summary>
/// Handler for processing the verification of a user by their authenticator token.
/// </summary>
// public class VerifyUserByAuthenticatorQueryHandler : IQueryHandler<VerifyUserByAuthenticatorQuery, VerifyUserResponseDto>
public class VerifyUserByAuthenticatorQueryHandler : TransactionalQueryHandler<VerifyUserByAuthenticatorQuery, VerifyUserResponseDto>
{
    private readonly IMfaRepository _mfaRepository;
    private readonly IMailRepository _mailRepository;
    private readonly IJwtRepository _jwtTokenRepository;
    private readonly IMapper _mapper;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="VerifyUserByAuthenticatorQueryHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work instance used to manage transactions and interact with repositories.</param>
    /// <param name="mfaRepository">Repository for managing multi-factor authentication (MFA) processes.</param>
    /// <param name="mailRepository">Repository for handling email-related operations.</param>
    /// <param name="jwtTokenRepository">Repository for handling JWT token generation and validation.</param>
    /// <param name="mapper">The mapper for converting between domain models and DTOs.</param>
    public VerifyUserByAuthenticatorQueryHandler(
        IUnitOfWork unitOfWork,
        IMfaRepository mfaRepository,
        IMailRepository mailRepository,
        IJwtRepository jwtTokenRepository,
        IMapper mapper
    ) : base(unitOfWork)
    {
        _mfaRepository = mfaRepository;
        _mailRepository = mailRepository;
        _jwtTokenRepository = jwtTokenRepository;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Handles the verification process for a user using the authenticator token and URL token.
    /// </summary>
    /// <param name="request">The query containing the authenticator token and URL token for verification.</param>
    /// <param name="cancellationToken">The cancellation token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="VerifyUserResponseDto"/> containing the verification result.</returns>
    /// <exception cref="BadRequestException">Thrown when the email confirmation token or MFA validation fails.</exception>
    // public async Task<VerifyUserResponseDto> Handle(VerifyUserByAuthenticatorQuery request, CancellationToken cancellationToken)
    // {
    //     Result<string> verifyEmailConfirmationTokenResult = await _mailRepository.VerifyEmailConfirmationToken(request.UrlToken);
    //
    //     if (verifyEmailConfirmationTokenResult.Succeeded)
    //     {
    //         Result validateMfaResult = await _mfaRepository.ValidateMfa(verifyEmailConfirmationTokenResult.Value!, request.AuthenticatorToken);
    //
    //         if (!validateMfaResult.Succeeded)
    //             throw new BadRequestException(validateMfaResult.Errors.Any() 
    //                 ? validateMfaResult.Errors.First() 
    //                 : "Something went wrong");
    //         
    //         var tokenModel = await _jwtTokenRepository.GenerateAuthResponseWithRefreshTokenCookie(verifyEmailConfirmationTokenResult.Value!);
    //         return _mapper.Map<VerifyUserResponseDto>(tokenModel);
    //     }
    //     
    //     throw new BadRequestException("Something went wrong");
    //     
    // }

    protected override async Task<VerifyUserResponseDto> ExecuteCoreAsync(VerifyUserByAuthenticatorQuery request, CancellationToken cancellationToken)
    {
        Result<UserAccountAggregate> verifyEmailConfirmationTokenResult = await _mailRepository.VerifyEmailConfirmationToken(request.UrlToken);

        if (verifyEmailConfirmationTokenResult.Succeeded)
        {
            Result validateMfaResult = await _mfaRepository.ValidateMfa(verifyEmailConfirmationTokenResult.Value!, request.AuthenticatorToken);

            if (!validateMfaResult.Succeeded)
                throw new BadRequestException(validateMfaResult.Errors.Any() 
                    ? validateMfaResult.Errors.First() 
                    : "Something went wrong");
            
            // var tokenModel = await _jwtTokenRepository.GenerateAuthResponseWithRefreshTokenCookie(verifyEmailConfirmationTokenResult.Value!);
            // return _mapper.Map<VerifyUserResponseDto>(tokenModel);
            return new VerifyUserResponseDto{Message = "VerifyUserResponseDto"};
        }
        
        throw new BadRequestException("Something went wrong");
    }
}