namespace Shared.HttpResponses;

/// <summary>
/// Represents a response with a 200 OK status code, commonly used when the request was successful.
/// </summary>
public class ApiOkResponse : ApiResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiOkResponse"/> class with a 200 status code.
    /// </summary>
    public ApiOkResponse()
        : base(200)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiOkResponse"/> class with a 200 status code and the provided data.
    /// </summary>
    /// <param name="data">The data to include in the response.</param>
    public ApiOkResponse(object? data)
        : base(200, null, data)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiOkResponse"/> class with a 200 status code, the provided data, and a custom message.
    /// </summary>
    /// <param name="data">The data to include in the response.</param>
    /// <param name="message">The custom message to include in the response.</param>
    public ApiOkResponse(object? data, string? message)
        : base(200, message, data)
    {
    }
}
