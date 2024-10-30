using MediatR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Behaviors;

public class LoggingBehaviors<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<LoggingBehaviors<TRequest, TResponse>> _logger;
    private const string LogTemplate = "{Action}: {HandlerName}";
    public LoggingBehaviors(ILogger<LoggingBehaviors<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

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