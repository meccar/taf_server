using MediatR;
using Microsoft.AspNetCore.Mvc;
using taf_server.Presentations.Dtos.Authentication;
// using taf_server.Presentations.Usecases.Authentication;
using Swashbuckle.AspNetCore.Annotations;
using taf_server.Application.Commands.Auth.Register;
using taf_server.Application.Exceptions;
using taf_server.Presentations.HttpResponss;

namespace taf_server.Presentations.Controllers.Authentication;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthenticationController(
    ILogger<AuthenticationController> logger,
    IMediator mediator)
    : ControllerBase
{
    // private readonly RegisterUseCase _registerUseCase;

    // RegisterUseCase registerUseCase
    // _registerUseCase = registerUseCase;

    [HttpPost("register")]
    [SwaggerOperation(
        Summary = "Register a new user",
        Description =
            "Registers a new user with the provided details. Returns a sign-in response upon successful registration."
    )]
    [SwaggerResponse(200, "User successfully registered", typeof(RegisterUserResponseDto))]
    [SwaggerResponse(400, "Invalid user input")]
    [SwaggerResponse(500, "An error occurred while processing the request")]
    //[ApiValidationFilter]
    public async Task<ActionResult> Register([FromBody] RegisterUserRequestDto registerDto)
    {
        logger.LogInformation("START: SignUp");
        try
        {
            var registerResponse = await mediator.Send(new RegisterCommand(registerDto));

            logger.LogInformation("END: SignUp");

            return Ok(new ApiOkResponse(registerResponse));
        }
        catch (BadRequestException e)
        {
            return BadRequest(new ApiBadRequestResponse(e.Message));
        }
        // catch (Exception)
        // {
        //     throw;
        // }
    }
}
