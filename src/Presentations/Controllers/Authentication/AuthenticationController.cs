using System.Security.Claims;
using Application.Commands.Auth.Delete;
using Application.Commands.Auth.Register;
using Application.Commands.UserAccount;
using Application.Queries.Auth.GetNewVerificationToken;
using Application.Queries.Auth.Login;
using Application.Queries.Auth.VerifyUser;
using Asp.Versioning;
using Infrastructure.Decorators.Guards;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.Authentication.Credentials;
using Shared.Dtos.Authentication.Login;
using Shared.Dtos.Authentication.Register;
using Shared.Dtos.UserAccount;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentations.Controllers.Authentication;

/// <summary>
/// Provides authentication endpoints for user registration, login, and verification.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
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
    /// Verifies the user using an authenticator (such as two-factor authentication).
    /// </summary>
    /// <param name="userEmailRequestDto">The request containing the email verification data.</param>
    /// <param name="userByAuthenticatorRequestDto">The request containing the authenticator verification data.</param>
    /// <returns>A response indicating whether the verification succeeded.</returns>
    [HttpPost("verify/mail/{UrlToken}")]
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
    
    [HttpPatch("update/{eid}")]
    [SwaggerOperation(
        Summary = "Update a user",
        Description =
            "Update a user with the provided details. Returns a sign-in response upon successful registration."
    )]
    [SwaggerResponse(201, "User successfully registered", typeof(UpdateUserAccountResponseDto))]
    [SwaggerResponse(400, "Invalid user input")]
    [SwaggerResponse(500, "An error occurred while processing the request")]
    [UserGuard]
    public async Task<ActionResult<UpdateUserAccountResponseDto>> UpdateUserAccount(
        [FromRoute] string eid,
        [FromBody] UpdateUserAccountRequestDto updateUserAccountRequestDto
    )
    {
        _logger.LogInformation($"START: Updating user account");

        var response = await _mediator.Send(new UpdateUserAccountCommand(updateUserAccountRequestDto, eid));
        
        _logger.LogInformation($"END: User account updated");

        return Ok(response);
    }
    
    [HttpDelete("delete/{eid}")]
    [SwaggerOperation(
        Summary = "Delete User",
        Description = "Returns a JSON object indicating if user is an admin"        
    )]
    [SwaggerResponse(200, "Successfully deleted user", typeof(object))]
    [SwaggerResponse(401, "Unauthorized")]
    [SwaggerResponse(500, "An error occurred while processing the request")]
    [UserGuard]
    public async Task<ActionResult> DeleteUser([FromRoute] string eid)
    {
        _logger.LogInformation("START: Delete User");

        var response = await _mediator.Send(new DeleteUserCommand(eid));
        
        _logger.LogInformation("END: Delete User");
        
        return Ok(response);
    }

    [HttpGet("get/token-verification-email/{eid}")]
    [SwaggerOperation(
        Summary = "Get Email verification token",
        Description = "Returns a JSON object indicating if new token verification has been sent"        
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns token email")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the request")]
    public async Task<ActionResult> GetNewVerificationToken([FromRoute] string eid)
    {
        _logger.LogInformation("START: Get New verification token");

        var response = await _mediator.Send(new GetNewVerificationTokenQuery(eid));
        
        _logger.LogInformation("END: Get New verification token");

        return Ok(response);
    }
}
