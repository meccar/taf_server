using MediatR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Behaviors;

/// <summary>
/// A pipeline behavior that handles exceptions occurring in the processing of a request.
/// Logs the exception details and augments the exception with request-specific data.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class ExceptionBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<ExceptionBehavior<TRequest, TResponse>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="logger">The logger to use for logging exceptions.</param>
    public ExceptionBehavior(ILogger<ExceptionBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles the execution of the pipeline, intercepting and logging any unhandled exceptions.
    /// </summary>
    /// <param name="request">The incoming request object.</param>
    /// <param name="next">The next delegate in the pipeline.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response object.</returns>
    /// <exception cref="Exception">The unhandled exception augmented with request-specific data.</exception>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception e)
        {
            var requestName = typeof(TRequest).Name;
            var requestType = typeof(TRequest).FullName;
            var responseType = typeof(TResponse).FullName;
            
            _logger.LogError(
                e,
                """
                Unhandled Exception:
                Request Name: {RequestName}
                Request Type: {RequestType}
                Response Type: {ResponseType}
                Request Details: {@Request}
                """,
                requestName,
                requestType,
                responseType,
                request);
            
            e.Data["RequestName"] = requestName;
            e.Data["RequestType"] = requestType;
            e.Data["ResponseType"] = responseType;
            e.Data["Request"] = request;
            
            throw;
        }
    }
}