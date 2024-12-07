using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Shared.Dtos.Exceptions;

namespace Shared.HttpResponses;

/// <summary>
/// A custom exception filter that handles specific exceptions and returns appropriate HTTP responses.
/// </summary>
public class HttpExceptionFilter : IAsyncActionFilter
{
    private readonly ILogger<HttpExceptionFilter> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpExceptionFilter"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging error details.</param>
    public HttpExceptionFilter(ILogger<HttpExceptionFilter> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Executes the action and handles exceptions that occur during action execution.
    /// </summary>
    /// <param name="context">The context for the current action being executed.</param>
    /// <param name="next">The delegate to the next action filter in the pipeline.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        try
        {
            await next();
        }
        catch (BadRequestException e)
        {
            _logger.LogError(e, "Bad request occurred.");
            context.Result = new BadRequestObjectResult(new ApiBadRequestResponse(e.Message));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unexpected error occurred.");
            context.Result = new StatusCodeResult(500);
        }
    }
}