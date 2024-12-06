using AutoMapper;
using Domain.Interfaces;
using Domain.SeedWork.Query;
using Shared.Dtos.Authentication.Login;
using Shared.Dtos.Exceptions;
using Shared.Model;

namespace Application.Queries.Auth.Login;

/// <summary>
/// Handles the login query by validating user credentials and generating an authentication response.
/// This handler processes the <see cref="LoginQuery"/>, validates the login credentials, and generates an authentication response 
/// containing a JWT token and refresh token for the user.
/// </summary>
public class LoginQueryHandler : IQueryHandler<LoginQuery, LoginResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtRepository _jwtTokenRepository;
    private readonly IMapper _mapper;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="LoginQueryHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work interface to interact with the data repositories.</param>
    /// <param name="jwtTokenRepository">The JWT repository responsible for generating authentication tokens.</param>
    /// <param name="mapper">The AutoMapper instance to map the token model to the response DTO.</param>
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
    public async Task<LoginResponseDto> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        // Validate user login data
        if (!await _unitOfWork.UserAccountRepository.ValidateUserLoginData(request.Email, request.Password))
            throw new UnauthorizedException("Invalid credentials");
        
        // Clear password after validation
        request.Password = null!;

        // Generate authentication token and refresh token
        var tokenModel = await _jwtTokenRepository.GenerateAuthResponseWithRefreshTokenCookie(request.Email);
        
        // Map the token model to a response DTO
        return _mapper.Map<LoginResponseDto>(tokenModel); 
    }
}