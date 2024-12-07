using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Shared.Dtos.Exceptions;

namespace Shared.HttpResponses;

public class HttpExceptionFilter : IAsyncActionFilter
{
    private readonly ILogger<HttpExceptionFilter> _logger;

    public HttpExceptionFilter(ILogger<HttpExceptionFilter> logger)
    {
        _logger = logger;
    }

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