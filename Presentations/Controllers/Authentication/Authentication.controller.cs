using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using taf_server.Presentations.Dtos.Authentication;
using Swashbuckle.AspNetCore.Annotations;
using taf_server.Application.Commands.Auth.Register;
using taf_server.Application.Exceptions;
using taf_server.Application.Queries.Auth.Login;
using taf_server.Presentations.Dtos.Authentication.Login;
using taf_server.Presentations.Dtos.Authentication.Register;
using taf_server.Presentations.Dtos.UserAccount;
using taf_server.Presentations.HttpResponse;
using taf_server.Presentations.HttpResponss;

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
    private readonly IMediator _mediator;
    public AuthenticationController(
        IMapper mapper,
        ILogger<AuthenticationController> logger,
        IMediator mediator)
    {
        _mapper = mapper;
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
        _logger.LogInformation("START: SignUp");
        
        var registerResponse = await _mediator.Send(new RegisterCommand(registerDto));
        
        _logger.LogInformation("END: SignUp");
        
        return Created(registerResponse.Uuid, _mapper.Map<RegisterUserResponseDto>(registerResponse));
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
        _logger.LogInformation("START: SignUp");
        
        var loginResponse = await _mediator.Send(new LoginQuery(loginDto));
        
        _logger.LogInformation("END: SignUp");
        
        return Ok(_mapper.Map<LoginResponseDto>(loginResponse));
    }
}
