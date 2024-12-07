using MediatR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Behaviors;

/// <summary>
/// A pipeline behavior for logging the start and end of request handling.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class LoggingBehaviors<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<LoggingBehaviors<TRequest, TResponse>> _logger;
    private const string LogTemplate = "{Action}: {HandlerName}";
    
    /// <summary>
    /// Initializes a new instance of the <see cref="LoggingBehaviors{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="logger">The logger to use for logging messages.</param>
    public LoggingBehaviors(ILogger<LoggingBehaviors<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles the execution of the pipeline, logging the start and end of request handling.
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
        _logger.LogInformation(LogTemplate, "BEGIN", typeof(TRequest).Name);
        
        var response = await next();
        
        _logger.LogInformation(LogTemplate, "END", typeof(TRequest).Name);
        
        return response;
    }
}