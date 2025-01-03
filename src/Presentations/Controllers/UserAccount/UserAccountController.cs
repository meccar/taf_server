using Application.Queries.UserAccount;
using Asp.Versioning;
using Infrastructure.Decorators.Guards;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.UserAccount;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentations.Controllers.UserAccount;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UserAccountController : ControllerBase
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

    [HttpGet("profile")]
    [SwaggerOperation(
        Summary = "Retrieves the current user's profile information",
        Description = "Returns detailed profile information for the authenticated user including personal details, preferences, and account settings"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Profile retrieved successfully", typeof(GetProfileResponseDto))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Authentication required. Please provide valid credentials.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Profile not found for the authenticated user.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An unexpected error occurred while processing your request.")]
    [UserGuard]
    public async Task<ActionResult<GetProfileResponseDto>> GetProfile()
    {
        _logger.LogInformation("START: Get profile");

        var response = await _mediator.Send(new GetProfileQuery());
            
        _logger.LogInformation("END: Get profile");
        
        return Ok(response);
    }
}