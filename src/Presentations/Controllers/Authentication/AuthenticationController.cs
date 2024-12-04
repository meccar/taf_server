using System.Security.Claims;
using Application.Usecases.Auth;
using Asp.Versioning;
using Infrastructure.Decorators.Guards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.Authentication.Credentials;
using Shared.Dtos.Authentication.Login;
using Shared.Dtos.Authentication.Register;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentations.Controllers.Authentication;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}")]
// [Route("api/v{version:apiVersion}/[controller]")]

public class AuthenticationController
    : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly RegisterUsecase _registerUseCase;
    private readonly LoginUsecase _loginUsecase;
    private readonly VerifyUserUsecase _verifyUserUsecase;
    public AuthenticationController(
        ILogger<AuthenticationController> logger,
        RegisterUsecase registerUsecase,
        LoginUsecase loginUsecase,
        VerifyUserUsecase verifyUserUsecase
        )
    {
        _logger = logger;
        _registerUseCase = registerUsecase;
        _loginUsecase = loginUsecase;
        _verifyUserUsecase = verifyUserUsecase;
    }

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

        var response = await _registerUseCase.Execute(registerDto);

        _logger.LogInformation("END: Register");

        return Created(response.Eid, response);
    }

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
    //[ApiValidationFilter]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginUserRequestDto loginDto)
    {
        _logger.LogInformation("START: Login");

        var response = await _loginUsecase.Execute(loginDto);
        
        _logger.LogInformation("END: Login");
        
        return Ok(response);
    }
    
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

    [HttpGet("verify/mail")]
    [SwaggerOperation(
        Summary = "Verify User",
        Description = "Returns a JSON object indicating if email exists"
    )]
    [SwaggerResponse(200, "Successfully verify user", typeof(object))]
    [SwaggerResponse(400, "Unauthorized")]
    // [UserGuard]
    public async Task<IActionResult> VerifyUser([FromQuery] VerifyUserRequestDto tokenRequestDto)
    {
        _logger.LogInformation("START: Verify User");

        var response = await _verifyUserUsecase.Execute(tokenRequestDto);
        
        _logger.LogInformation("END: Verify User");
        
        return Ok(response);
    }
}
