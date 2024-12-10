using System.Security.Claims;
using Application.Commands.Auth.Register;
using Application.Queries.Auth.Login;
using Application.Queries.Auth.VerifyUser;
using Application.Queries.Auth.VerifyUserEmail;
using Asp.Versioning;
using Infrastructure.Decorators.Guards;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.Authentication.Credentials;
using Shared.Dtos.Authentication.Login;
using Shared.Dtos.Authentication.Register;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentations.Controllers.Authentication;

/// <summary>
/// Provides authentication endpoints for user registration, login, and verification.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}")]
// [Route("api/v{version:apiVersion}/[controller]")]

public class AuthenticationController
    : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationController"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging activities.</param>
    /// <param name="mediator">The mediator to send commands and queries to.</param>
    public AuthenticationController(
        ILogger<AuthenticationController> logger,
        IMediator mediator
        )
    {
        _logger = logger;
        _mediator = mediator;
    }

    /// <summary>
    /// Registers a new user with the provided details.
    /// </summary>
    /// <param name="registerDto">The user registration data.</param>
    /// <returns>A response containing the created user information.</returns>
    [HttpPost("register")]
    [SwaggerOperation(
        Summary = "Register a new user",
        Description =
            "Registers a new user with the provided details. Returns a sign-in response upon successful registration."
    )]
    [SwaggerResponse(201, "User successfully registered", typeof(RegisterUserResponseDto))]
    [SwaggerResponse(400, "Invalid user input")]
    [SwaggerResponse(500, "An error occurred while processing the request")]
    [AllowAnonymous]
    //[ApiValidationFilter]
    public async Task<ActionResult<RegisterUserResponseDto>> Register([FromBody] RegisterUserRequestDto registerDto)
    {
        _logger.LogInformation("START: Register");

        var loginResponse = await _mediator.Send(new RegisterCommand(registerDto.UserProfile, registerDto.UserAccount));
        
        _logger.LogInformation("END: Register");

        return Created(loginResponse.Eid, loginResponse);
    }

    /// <summary>
    /// Logs in a user with the provided credentials.
    /// </summary>
    /// <param name="loginDto">The login credentials.</param>
    /// <returns>A response containing the login token and user details.</returns>
    [HttpPost("login")]
    [SwaggerOperation(
        Summary = "Login as a user",
        Description =
            "Login as a user with the provided details. Returns a sign-in response upon successful registration."
    )]
    [SwaggerResponse(200, "User successfully logined", typeof(LoginUserRequestDto))]
    [SwaggerResponse(400, "Invalid user input")]
    [SwaggerResponse(500, "An error occurred while processing the request")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginUserRequestDto loginDto)
    {
        _logger.LogInformation("START: Login");

        var response = await _mediator.Send(new LoginQuery(loginDto));
        
        _logger.LogInformation("END: Login");
        
        return Ok(response);
    }
    
    /// <summary>
    /// Checks if the currently authenticated user is an admin.
    /// </summary>
    /// <returns>A JSON object indicating if the user is an admin.</returns>
    [HttpGet("admin")]
    [SwaggerOperation(
        Summary = "Get Admin Status",
        Description = "Returns a JSON object indicating if the user is an admin"
    )]
    [SwaggerResponse(200, "Successfully fetched admin status", typeof(object))]
    [SwaggerResponse(401, "Unauthorized")]
    [AdminGuard]
    public IActionResult GetAdminStatus()
    {
        var claims = User.Claims.ToList();
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userName = User.FindFirst(ClaimTypes.Name)?.Value;
        var sessionId = User.FindFirst("session_id")?.Value;
        
        return Ok(new {
            UserId = userId,
            UserName = userName,
            SessionId = sessionId,
            AllClaims = claims
        });
    }

    /// <summary>
    /// Verifies if the user's email exists in the system.
    /// </summary>
    /// <param name="tokenRequestDto">The request containing the email verification token.</param>
    /// <returns>A response indicating whether the user exists or not.</returns>
    [HttpGet("verify/mail")]
    [SwaggerOperation(
        Summary = "Verify User",
        Description = "Returns a JSON object indicating if email exists"
    )]
    [SwaggerResponse(200, "Successfully verify user", typeof(object))]
    [SwaggerResponse(400, "Unauthorized")]
    // [UserGuard]
    public async Task<IActionResult> VerifyUserEmail([FromQuery] VerifyUserEmailRequestDto tokenRequestDto)
    {
        _logger.LogInformation("START: Verify User");
    
        var response = await _mediator.Send(new VerifyUserEmailQuery(tokenRequestDto));
        
        _logger.LogInformation("END: Verify User");
        
        return Ok(response);
    }
    
    /// <summary>
    /// Verifies the user using an authenticator (such as two-factor authentication).
    /// </summary>
    /// <param name="userEmailRequestDto">The request containing the email verification data.</param>
    /// <param name="userByAuthenticatorRequestDto">The request containing the authenticator verification data.</param>
    /// <returns>A response indicating whether the verification succeeded.</returns>
    [HttpPost("verify/mail")]
    [SwaggerOperation(
        Summary = "Verify User",
        Description = "Returns a JSON object indicating if email exists"
    )]
    [SwaggerResponse(200, "Successfully verify user", typeof(object))]
    [SwaggerResponse(400, "Unauthorized")]
    // [UserGuard]
    public async Task<IActionResult> VerifyUser(
        [FromQuery] VerifyUserEmailRequestDto userEmailRequestDto,
        [FromBody] VerifyUserByAuthenticatorRequestDto userByAuthenticatorRequestDto
    )
    {
        _logger.LogInformation("START: Verify User");

        var response = await _mediator.Send(new VerifyUserByAuthenticatorQuery(userEmailRequestDto, userByAuthenticatorRequestDto));
        
        _logger.LogInformation("END: Verify User");
        
        return Ok(response);
    }
}
