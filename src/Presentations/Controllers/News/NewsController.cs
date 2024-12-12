using Application.Commands.News;
using Application.Queries.News;
using Asp.Versioning;
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
    // [UserGuard]
    [AllowAnonymous]
    public async Task<ActionResult<CreateNewsResponseDto>> Create(
        [FromBody] CreateNewsRequestDto createNewsRequestDto
        )
    {
        _logger.LogInformation("START: Create News");
        
        var createNewsResponse = await _mediator.Send(new CreateNewsCommand(createNewsRequestDto));
        
        _logger.LogInformation("END: Create News");
        
        return Created(createNewsResponse.Uuid, createNewsResponse);
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

        var getAllNewsResponse = await _mediator.Send(new GetAllNewsQuery(paginationParams));
        
        _logger.LogInformation("END: Get All News");

        return Ok(getAllNewsResponse);
    }
    
    [HttpGet("{Eid}")]
    [SwaggerOperation(
        Summary = "Get detail News",
        Description = "Get detail News"
    )]
    [SwaggerResponse(200, "Get detail news", typeof(GetAllNewsResponseDto))]
    [SwaggerResponse(400, "Invalid News input")]
    [SwaggerResponse(500, "An error occured while processing the request")]
    [AllowAnonymous]
    public async Task<ActionResult<GetDetailNewsResponseDto>> GetDetailNews(
        [FromQuery] GetDetailNewsRequestDto getDetailNewsRequestDto
        )
    {
        _logger.LogInformation("START: Get Detail News");

        var getDetailNewsResponse = await _mediator.Send(new GetDetailNewsQuery(getDetailNewsRequestDto));
        
        _logger.LogInformation("END: Get Detail News");

        return Ok(getDetailNewsResponse);
    }
}