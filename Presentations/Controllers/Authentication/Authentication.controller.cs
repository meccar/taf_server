using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using taf_server.Presentations.Dtos.Authentication.Login;
using taf_server.Presentations.Dtos.Authentication.Register;
using taf_server.Application.Usecases.Auth;
using taf_server.Infrastructure.UseCaseProxy;

namespace taf_server.Presentations.Controllers.Authentication;

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
    //[ApiValidationFilter]
    public async Task<ActionResult<RegisterUserResponseDto>> Register([FromBody] RegisterUserRequestDto registerDto)
    {
        _logger.LogInformation("START: Register");

        var response = await _registerUseCase.GetInstance().Execute(registerDto);

        _logger.LogInformation("END: Register");

        return Created(response.Uuid, response);
    }

    [HttpGet("login")]
    [SwaggerOperation(
        Summary = "Login as a user",
        Description =
            "Login as a user with the provided details. Returns a sign-in response upon successful registration."
    )]
    [SwaggerResponse(200, "User successfully logined", typeof(LoginUserRequestDto))]
    [SwaggerResponse(400, "Invalid user input")]
    [SwaggerResponse(500, "An error occurred while processing the request")]
    //[ApiValidationFilter]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginUserRequestDto loginDto)
    {
        _logger.LogInformation("START: Login");

        var response = await _loginUsecase.GetInstance().Execute(loginDto);

        _logger.LogInformation("END: Login");

        return Ok(response);
    }
}
