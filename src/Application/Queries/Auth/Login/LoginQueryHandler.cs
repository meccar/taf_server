using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.Credentials;
using Domain.Interfaces.Tokens;
using Domain.SeedWork.Query;
using Shared.Dtos.Authentication.Login;
using Shared.Dtos.Exceptions;

namespace Application.Queries.Auth.Login;

/// <summary>
/// Handles the login query by validating user credentials and generating an authentication response.
/// This handler processes the <see cref="LoginQuery"/>, validates the login credentials, and generates an authentication response 
/// containing a JWT token and refresh token for the user.
/// </summary>
public class LoginQueryHandler : TransactionalQueryHandler<LoginQuery, LoginResponseDto>
{
    private readonly IJwtRepository _jwtTokenRepository;
    private readonly IMapper _mapper;
    private readonly ISignInRepository _signInRepository;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="LoginQueryHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work for handling transactions.</param>
    /// <param name="jwtTokenRepository">The JWT repository responsible for generating authentication tokens.</param>
    /// <param name="mapper">The AutoMapper instance to map models to response DTOs.</param>
    /// <param name="signInRepository">The repository responsible for handling user sign-in operations.</param>
    public LoginQueryHandler(
        IUnitOfWork unitOfWork,
        IJwtRepository jwtTokenRepository,
        IMapper mapper,
        ISignInRepository signInRepository
        ) : base(unitOfWork)
    {
        _jwtTokenRepository = jwtTokenRepository;
        _mapper = mapper;
        _signInRepository = signInRepository;
    }

    /// <summary>
    /// Handles the <see cref="LoginQuery"/>, validates the user's login credentials, and returns a <see cref="LoginResponseDto"/> 
    /// containing the authentication token and refresh token if successful.
    /// </summary>
    /// <param name="request">The login query containing the user's email and password for authentication.</param>
    /// <param name="cancellationToken">The cancellation token to monitor for task cancellation.</param>
    /// <returns>A <see cref="LoginResponseDto"/> containing the JWT token and refresh token if the credentials are valid.</returns>
    /// <exception cref="UnauthorizedException">Thrown when the user’s credentials are invalid.</exception>
    /// <remarks>
    /// This method first validates the user’s credentials using the <see cref="IUnitOfWork"/>'s UserAccountRepository.
    /// If the credentials are valid, it generates an authentication response that includes a JWT token and refresh token,
    /// using the <see cref="IJwtRepository"/>.
    /// </remarks>
    protected override async Task<LoginResponseDto> ExecuteCoreAsync(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await UnitOfWork.UserAccountRepository.GetUserByEmail(request.Email);
        
        if (user is null)
            throw new BadRequestException("Could not verify your account");
        
        var result = await _signInRepository
            .SignInAsync(
                user,
                request.Password,
                false
            );
        
        request.Password = null!;
        
        if (result is null)
            throw new UnauthorizedException("Invalid credentials");
            
        // Generate authentication token and refresh token
        var tokenGenerationResult = await _jwtTokenRepository
            .GenerateAuthResponseWithRefreshTokenCookie(
                result.Value.Item1,
                result.Value.Item2
            );
        
        return tokenGenerationResult is not null
            ? _mapper.Map<LoginResponseDto>(tokenGenerationResult) 
            : throw new UnauthorizedException("Invalid credentials");
    }
}