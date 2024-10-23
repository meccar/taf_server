using taf_server.Presentations.HttpResponss;

namespace taf_server.Presentations.HttpResponse;

public class ApiCreatedResponse : ApiResponse
{
    public ApiCreatedResponse()
        : base(201)
    {
    }

    public ApiCreatedResponse(object? data)
        : base(201, null, data)
    {
    }

    public ApiCreatedResponse(object? data, string? message)
        : base(201, message, data)
    {
    }
}