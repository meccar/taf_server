using Application.Commands.UserProfile;
using Application.Queries.UserProfile;
using Asp.Versioning;
using Infrastructure.Decorators.Guards;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.Pagination;
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
    [SwaggerResponse(StatusCodes.Status201Created, "User successfully registered", typeof(UpdateUserProfileResponseDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid user input")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the request")]
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

    [HttpGet("")]
    [SwaggerOperation(
        Summary = "Get all user information",
        Description = "Get all user information"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Get all user information", typeof(GetAllUserProfileResponseDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid user input")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the request")]
    [AdminGuard]
    public async Task<ActionResult<PaginationResponse<GetAllUserProfileResponseDto>>> GetAll(
        [FromQuery] PaginationParams paginationParams
    )
    {
        _logger.LogInformation($"START: Get all user information");
        
        var response = await _mediator.Send(new GetAllUserProfileQuery(paginationParams));
        
        _logger.LogInformation("END: Get all user information");
        
        return Ok(response);
    }

    [HttpGet("{eid}")]
    [SwaggerOperation(
        Summary = "Get user information",
        Description = "Get user information"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved user's data",
        typeof(GetDetailUserProfileResponseDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid user input")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the request")]
    [AdminGuard]
    public async Task<ActionResult<GetDetailUserProfileResponseDto>> GetDetailUserProfile(
        [FromRoute] string eid
    )
    {
        _logger.LogInformation($"START: Get user detail information");

        var response = await _mediator.Send(new GetDetailUserProfileQuery(eid));
        
        _logger.LogInformation("END: Get user detail information");
        
        return Ok(response);
    }
}