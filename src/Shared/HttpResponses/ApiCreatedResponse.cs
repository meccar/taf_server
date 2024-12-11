namespace Shared.HttpResponses;

/// <summary>
/// Represents a response with a 201 Created status code, commonly used for successful creation operations.
/// </summary>
public class ApiCreatedResponse : ApiResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiCreatedResponse"/> class with a 201 status code.
    /// </summary>
    public ApiCreatedResponse()
        : base(201)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiCreatedResponse"/> class with a 201 status code and a specified data payload.
    /// </summary>
    /// <param name="data">The data to be included in the response payload.</param>
    public ApiCreatedResponse(object? data)
        : base(201, null, data)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiCreatedResponse"/> class with a 201 status code, a specified message, and data payload.
    /// </summary>
    /// <param name="data">The data to be included in the response payload.</param>
    /// <param name="message">The custom message to be included in the response.</param>
    public ApiCreatedResponse(object? data, string? message)
        : base(201, message, data)
    {
    }
}