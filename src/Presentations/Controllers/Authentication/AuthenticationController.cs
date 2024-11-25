using System.Security.Claims;
using Application.Dtos.Authentication.Login;
using Application.Dtos.Authentication.Register;
using Application.Usecases.Auth;
using Asp.Versioning;
using AutoMapper;
using Infrastructure.Decorators.Guards;
using Infrastructure.UseCaseProxy;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentations.Controllers.Authentication;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}")]
// [Route("api/v{version:apiVersion}/[controller]")]

public class AuthenticationController
    : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILogger<AuthenticationController> _logger;
    private readonly UseCaseProxy<RegisterUsecase, RegisterUserRequestDto, RegisterUserResponseDto> _registerUseCase;
    private readonly UseCaseProxy<LoginUsecase, LoginUserRequestDto, LoginResponseDto> _loginUsecase;
    private readonly IMediator _mediator;
    public AuthenticationController(
        IMapper mapper,
        UseCaseProxy<RegisterUsecase, RegisterUserRequestDto, RegisterUserResponseDto> registerUsecase,
        UseCaseProxy<LoginUsecase, LoginUserRequestDto, LoginResponseDto> loginUsecase,
        ILogger<AuthenticationController> logger,
        IMediator mediator)
    {
        _mapper = mapper;
        _registerUseCase = registerUsecase;
        _loginUsecase = loginUsecase;
        _logger = logger;
        _mediator = mediator;
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

        var response = await _registerUseCase.GetInstance().Execute(registerDto);

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

        var response = await _loginUsecase.GetInstance().Execute(loginDto);
        
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
}