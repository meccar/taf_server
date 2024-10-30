using MediatR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Behaviors;

public class ExceptionBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<ExceptionBehavior<TRequest, TResponse>> _logger;

    public ExceptionBehavior(ILogger<ExceptionBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

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