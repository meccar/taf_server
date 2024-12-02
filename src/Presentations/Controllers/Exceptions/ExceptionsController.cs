

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Dtos.Exceptions;
using Shared.HttpResponses;

namespace Presentations.Controllers.Exceptions;

public class ExceptionsController : IExceptionFilter
{
    private readonly ILogger<ExceptionsController> _logger;

    public ExceptionsController(ILogger<ExceptionsController> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "An unhandled exception occurred.");

        ApiResponse response = context.Exception switch
        {
            BadRequestException ex => new ApiBadRequestResponse(ex.Message),
            NotFoundException ex => new ApiNotFoundResponse(ex.Message),
            ForbiddenException ex => new ApiForbiddenResponse(ex.Message),
            UnauthorizedException ex => new ApiUnauthorizedResponse(ex.Message),
            _ => new ApiServerErrorResponse("An unexpected error occurred.")
        };

        context.Result = context.Exception switch
        {
            BadRequestException => new BadRequestObjectResult(response),
            NotFoundException => new NotFoundObjectResult(response),
            UnauthorizedException => new UnauthorizedObjectResult(response),
            ForbiddenException => new ObjectResult(response) { StatusCode = 403 },
            _ => new ObjectResult(response) { StatusCode = 500 }
        };

        context.ExceptionHandled = true;
    }
}