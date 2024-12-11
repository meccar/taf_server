using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Behaviors;

/// <summary>
/// A pipeline behavior for measuring and logging the execution time of requests.
/// Logs a warning if a request exceeds a specified execution time threshold.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class PerformanceBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly Stopwatch _timer;
    private readonly ILogger<ExceptionBehavior<TRequest, TResponse>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PerformanceBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="logger">The logger used for logging performance metrics.</param>
    public PerformanceBehavior(ILogger<ExceptionBehavior<TRequest, TResponse>> logger)
    {
        _timer = new Stopwatch();
        _logger = logger;
    }

    /// <summary>
    /// Handles the execution of the pipeline, measuring the execution time of the request.
    /// Logs a warning if the execution time exceeds 500 milliseconds.
    /// </summary>
    /// <param name="request">The incoming request object.</param>
    /// <param name="next">The next delegate in the pipeline.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response object from the pipeline.</returns>
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        _timer.Start();
        
        var response = await next();
        
        _timer.Stop();
        
        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds >= 500)
        {
            var requestName = typeof(TRequest).Name;
            
            _logger.LogWarning(
                "Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}",
                requestName, elapsedMilliseconds, request);
        }
        return response;
    }
}