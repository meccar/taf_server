using System.Runtime.CompilerServices;
using Application.Commands.News;
using Application.Commands.News.Create;
using Application.Commands.News.Delete;
using Application.Commands.News.Update;
using Application.Queries.News;
using Application.Queries.News.GetAll;
using Application.Queries.News.GetDetail;
using Asp.Versioning;
using Infrastructure.Decorators.Guards;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.News;
using Shared.Dtos.Pagination;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentations.Controllers.News;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]

public class NewsController
    : ControllerBase
{
    private readonly ILogger<NewsController> _logger;
    private readonly IMediator _mediator;

    public NewsController(
        ILogger<NewsController> logger,
        IMediator mediator
    )
    {
        _logger = logger;
        _mediator = mediator;
    }
    
    [HttpPost("create")]
    [SwaggerOperation(
        Summary = "Create a new News",
        Description = "Creates a new News with the provided details. Returns responses upon requests"
    )]
    [SwaggerResponse(201, "News successfully created", typeof(CreateNewsResponseDto))]
    [SwaggerResponse(400, "Invalid News input")]
    [SwaggerResponse(500, "An error occured while processing the request")]
    [UserGuard]
    public async Task<ActionResult<CreateNewsResponseDto>> Create(
        [FromBody] CreateNewsRequestDto createNewsRequestDto
        )
    {
        _logger.LogInformation("START: Create News");
        
        var response = await _mediator.Send(new CreateNewsCommand(createNewsRequestDto));
        
        _logger.LogInformation("END: Create News");
        
        return Created(response.Uuid, response);
    }

    [HttpGet("")]
    [SwaggerOperation(
        Summary = "Get All News",
        Description = "Get All News"
    )]
    [SwaggerResponse(200, "Get all news", typeof(GetAllNewsResponseDto))]
    [SwaggerResponse(400, "Invalid News input")]
    [SwaggerResponse(500, "An error occured while processing the request")]
    [AllowAnonymous]
    public async Task<ActionResult<PaginationResponse<GetAllNewsResponseDto>>> GetAll(
        [FromQuery] PaginationParams paginationParams
        )
    {
        _logger.LogInformation("START: Get All News");

        var response = await _mediator.Send(new GetAllNewsQuery(paginationParams));
        
        _logger.LogInformation("END: Get All News");

        return Ok(response);
    }
    
    [HttpGet("{eid}")]
    [SwaggerOperation(
        Summary = "Retrieve Detailed News Information",
        Description = "Fetches comprehensive details for a specific news item"
    )]
    [SwaggerResponse(200, "Successfully retrieved news details", typeof(GetDetailNewsResponseDto))]
    [SwaggerResponse(400, "Invalid request parameters")]
    [SwaggerResponse(500, "Internal server error")]
    [AllowAnonymous]
    public async Task<ActionResult<GetDetailNewsResponseDto>> GetDetailNews(
        [FromRoute] string eid)
    {
        _logger.LogInformation("START: Retrieving News Details");

        var response = await _mediator.Send(new GetDetailNewsQuery(eid));
        
        _logger.LogInformation("END: News Details Retrieved");

        return Ok(response);
    }
    [HttpPatch("update/{eid}")]
    [SwaggerOperation(
        Summary = "Update News Item",
        Description = "Updates an existing news item with provided information"
    )]
    [SwaggerResponse(200, "News successfully updated", typeof(UpdateNewsResponseDto))]
    [SwaggerResponse(400, "Invalid update parameters")]
    [SwaggerResponse(500, "Error during news update")]
    [UserGuard]
    public async Task<ActionResult<UpdateNewsResponseDto>> UpdateNews(
        [FromRoute] string eid,
        [FromBody] UpdateNewsRequestDto updateNewsRequestDto)
    {
        _logger.LogInformation("START: Updating News Item {eid}");

        var response = await _mediator.Send(new UpdateNewsCommand(updateNewsRequestDto, eid));
        
        _logger.LogInformation("END: News Item {eid} Updated");

        return Ok(response);
    }

    [HttpDelete("delete/{eid}")]
    [SwaggerOperation(
        Summary = "Delete News Item",
        Description = "Deletes an existing news item with provided information"
    )]
    [SwaggerResponse(200, "News successfully deleted", typeof(object))]
    [SwaggerResponse(400, "Invalid delete parameters")]
    [SwaggerResponse(500, "Internal server error")]
    [UserGuard]
    public async Task<ActionResult> DeleteNews(
        [FromRoute] string eid)
    {
        _logger.LogInformation("START: Deleting News Item");

        var response = await _mediator.Send(new DeleteNewsCommand(eid));
        
        _logger.LogInformation("END: News Item Deleted");
        
        return Ok(response);
    }
}