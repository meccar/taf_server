using AutoMapper;
using Domain.Aggregates;
using Domain.Interfaces;
using Domain.SeedWork.Query;
using Shared.Dtos.Authentication.Credentials;
using Shared.Dtos.Exceptions;
using Shared.Results;

namespace Application.Queries.Auth.VerifyUserEmail;

/// <summary>
/// Handles the verification of a user's email using a confirmation token.
/// This handler processes the <see cref="VerifyUserEmailQueryHandler"/>, validates the token for email confirmation,
/// and generates an authentication response containing a JWT token and refresh token if the token is valid.
/// </summary>
public class VerifyUserEmailQueryHandler : IQueryHandler<VerifyUserEmailQuery, VerifyUserResponseDto>
{
    private readonly IMailRepository _mailRepository;
    private readonly IJwtRepository _jwtTokenRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="VerifyUserEmailQueryHandler"/> class.
    /// </summary>
    /// <param name="mailRepository">The mail repository responsible for verifying email confirmation tokens.</param>
    /// <param name="jwtTokenRepository">The JWT repository responsible for generating authentication tokens.</param>
    /// <param name="mapper">The AutoMapper instance to map the token model to the response DTO.</param>
    public VerifyUserEmailQueryHandler(
        IMailRepository mailRepository,
        IJwtRepository jwtTokenRepository,
        IMapper mapper
    )
    {
        _mailRepository = mailRepository;
        _jwtTokenRepository = jwtTokenRepository;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Handles the <see cref="VerifyUserEmailQueryHandler"/>, validates the email confirmation token, and generates an authentication response.
    /// If the token is valid, it returns a <see cref="VerifyUserEmailRequestDto"/> containing the JWT token and refresh token.
    /// </summary>
    /// <param name="request">The query containing the email confirmation token to verify.</param>
    /// <param name="cancellationToken">The cancellation token to monitor for task cancellation.</param>
    /// <returns>A <see cref="VerifyUserEmailRequestDto"/> containing the JWT token and refresh token if the confirmation token is valid.</returns>
    /// <exception cref="BadRequestException">Thrown when the email confirmation token is invalid or verification fails.</exception>
    /// <remarks>
    /// This method first verifies the provided email confirmation token using the <see cref="IMailRepository"/>.
    /// If the token is valid, it generates a new authentication response, including a JWT token and refresh token, 
    /// using the <see cref="IJwtRepository"/>.
    /// </remarks>
    public async Task<VerifyUserResponseDto> Handle(VerifyUserEmailQuery request, CancellationToken cancellationToken)
    {
        // Verify the email confirmation token
        Result<UserAccountAggregate> result = await _mailRepository.VerifyEmailConfirmationToken(request.Token);

        if (result.Succeeded)
        {
            // Generate authentication response with refresh token if verification succeeds
            // var tokenModel = await _jwtTokenRepository.GenerateAuthResponseWithRefreshTokenCookie(result.Value!);
            // return _mapper.Map<VerifyUserResponseDto>(tokenModel);
            return new VerifyUserResponseDto{Message = "VerifyUserResponseDto"};

        }

        // Throw exception if verification fails
        throw new BadRequestException("Bad request");
    }
}