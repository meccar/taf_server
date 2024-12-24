using Application.Commands.UserProfile;
using Asp.Versioning;
using Infrastructure.Decorators.Guards;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.UserProfile;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentations.Controllers.UserProfile;


[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UserProfileController
    : ControllerBase
{
    private readonly ILogger<UserProfileController> _logger;
    private readonly IMediator _mediator;
    
    public UserProfileController(
        ILogger<UserProfileController> logger,
        IMediator mediator
    )
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPatch("update/{eid}")]
    [SwaggerResponse(201, "User successfully registered", typeof(UpdateUserProfileResponseDto))]
    [SwaggerResponse(400, "Invalid user input")]
    [SwaggerResponse(500, "An error occurred while processing the request")]
    [UserGuard]
    public async Task<ActionResult<UpdateUserProfileResponseDto>> UpdateUserProfile(
        [FromRoute] string eid,
        [FromBody] UpdateUserProfileRequestDto updateUserProfileRequestDto
    )
    {
        _logger.LogInformation($"START: Updating user profile");

        var response = await _mediator.Send(new UpdateUserProfileCommand(updateUserProfileRequestDto, eid));
        
        _logger.LogInformation($"END: User profile updated");

        return Ok(response);
    }
}