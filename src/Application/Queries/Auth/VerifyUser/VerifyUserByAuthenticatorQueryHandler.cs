using System.Text;
using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.Credentials;
using Domain.Interfaces.Tokens;
using Microsoft.AspNetCore.WebUtilities;
using Shared.Dtos.Authentication.Credentials;
using Shared.Dtos.Exceptions;

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
    private readonly ISignInRepository _signInRepository;
    private readonly IMapper _mapper;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="VerifyUserByAuthenticatorQueryHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work instance used to manage transactions and interact with repositories.</param>
    /// <param name="mfaRepository">Repository for managing multi-factor authentication (MFA) processes.</param>
    /// <param name="mailRepository">Repository for handling email-related operations.</param>
    /// <param name="jwtTokenRepository">Repository for generating and validating JWT tokens.</param>
    /// <param name="signInRepository">Repository for signing in users and managing authentication states.</param>
    /// <param name="mapper">Mapper for converting between domain models and DTOs.</param>
    public VerifyUserByAuthenticatorQueryHandler(
        IUnitOfWork unitOfWork,
        IMfaRepository mfaRepository,
        IMailRepository mailRepository,
        IJwtRepository jwtTokenRepository,
        ISignInRepository signInRepository,
        IMapper mapper
    ) : base(unitOfWork)
    {
        _mfaRepository = mfaRepository;
        _mailRepository = mailRepository;
        _jwtTokenRepository = jwtTokenRepository;
        _signInRepository = signInRepository;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Handles the verification process for a user using the authenticator token and URL token.
    /// </summary>
    /// <param name="request">The query containing the authenticator token and URL token for verification.</param>
    /// <param name="cancellationToken">The cancellation token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="VerifyUserResponseDto"/> containing the verification result.</returns>
    /// <exception cref="BadRequestException">Thrown when the email confirmation token or MFA validation fails.</exception>
    protected override async Task<VerifyUserResponseDto> ExecuteCoreAsync(
        VerifyUserByAuthenticatorQuery request,
        CancellationToken cancellationToken)
    {
        string decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.UrlToken));

        string[] parts = decodedToken.Split(':');

        if (parts.Length < 3)
            throw new BadRequestException("Could not verify your account");
            
        string email = parts[0];
        string userKey = parts[2];
        string emailKey = parts[3];
        
        var user = await UnitOfWork.UserAccountRepository.GetUserByEmail(email);
        
        if (user is null)
            throw new BadRequestException("Could not verify your account");

        var verifyEmailConfirmationTokenResult = await _mailRepository
            .VerifyEmailConfirmationToken(user, userKey, emailKey);

        if (verifyEmailConfirmationTokenResult is null)
            throw new BadRequestException("Something went wrong");   

        var validateMfaResult = await _mfaRepository
                .ValidateMfa(
                    verifyEmailConfirmationTokenResult,
                    request.AuthenticatorToken
                );

        if (!validateMfaResult)
            throw new BadRequestException("Invalid 2-factor token");
        
        var signInResult = await _signInRepository
            .SignInAsync(
                user,
                null!,
                false
            );
        
        if (signInResult is null)
            throw new BadRequestException("Failed while logging in, please try again.");
        
        var tokenModel = await _jwtTokenRepository
            .GenerateAuthResponseWithRefreshTokenCookie(
                signInResult.Value.Item1, signInResult.Value.Item2 
            );

        if (tokenModel is null)
            throw new BadRequestException("Failed while logging in, please try again.");
        
        return _mapper.Map<VerifyUserResponseDto>(tokenModel);
    }
}