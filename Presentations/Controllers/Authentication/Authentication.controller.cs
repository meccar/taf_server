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
    private readonly IMediator _mediator;
    public AuthenticationController(
        IMapper mapper,
        UseCaseProxy<RegisterUsecase, RegisterUserRequestDto, RegisterUserResponseDto> registerUsecase,
        ILogger<AuthenticationController> logger,
        IMediator mediator)
    {
        _mapper = mapper;
        _registerUseCase = registerUsecase;
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
    //public async Task<ActionResult<RegisterUserResponseDto>> Register([FromBody] RegisterUserRequestDto registerDto)
    //{
    //    _logger.LogInformation("START: Register");

    //    var registerResponse = await _mediator.Send(new RegisterCommand(registerDto));

    //    _logger.LogInformation("END: Register");

    //    return Created(registerResponse.Uuid, _mapper.Map<RegisterUserResponseDto>(registerResponse));
    //}
    public async Task<ActionResult<RegisterUserResponseDto>> Register([FromBody] RegisterUserRequestDto registerDto)
    {
        _logger.LogInformation("START: Register");

        var useCase = _registerUseCase.GetInstance();
        var response = await useCase.Execute(registerDto);

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
        
        var loginResponse = await _mediator.Send(new LoginQuery(loginDto));
        
        _logger.LogInformation("END: Login");
        
        return Ok(_mapper.Map<LoginResponseDto>(loginResponse));
    }
}
