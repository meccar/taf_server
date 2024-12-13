using Application.Commands.UserAccount;
using Asp.Versioning;
using Infrastructure.Decorators.Guards;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.Authentication.Register;
using Shared.Dtos.UserAccount;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentations.Controllers.UserAccount;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}")]
public class UserAccountController
    : ControllerBase
{
    private readonly ILogger<UserAccountController> _logger;
    private readonly IMediator _mediator;
    
    public UserAccountController(
        ILogger<UserAccountController> logger,
        IMediator mediator
    )
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPatch("register/{eid}")]
    [SwaggerOperation(
        Summary = "Register a new user",
        Description =
            "Registers a new user with the provided details. Returns a sign-in response upon successful registration."
    )]
    [SwaggerResponse(201, "User successfully registered", typeof(RegisterUserResponseDto))]
    [SwaggerResponse(400, "Invalid user input")]
    [SwaggerResponse(500, "An error occurred while processing the request")]
    [UserGuard]
    public async Task<ActionResult<UpdateUserAccountResponseDto>> UpdateUserAccount(
        [FromRoute] string eid,
        [FromBody] UpdateUserAccountRequestDto updateUserAccountRequestDto
    )
    {
        _logger.LogInformation($"START: Updating user with id: {eid}");

        var response = await _mediator.Send(new UpdateUserAccountCommand(updateUserAccountRequestDto, eid));
        
        _logger.LogInformation($"END: User with id {eid} updated");

        return Ok(response);
    }
}