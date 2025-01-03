using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Shared.HttpResponses;

/// <summary>
/// Represents a standard API response.
/// </summary>
public class ApiResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiResponse"/> class.
    /// </summary>
    /// <param name="statusCode">The HTTP status code of the response.</param>
    /// <param name="message">An optional message describing the response.</param>
    /// <param name="data">Optional data associated with the response.</param>
    public ApiResponse(int statusCode, string? message = null, object? data = null)
    {
        StatusCode = statusCode;
        Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        Data = data;
    }

    /// <summary>
    /// Gets the HTTP status code of the response.
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Gets the message associated with the response.
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Message { get; }
    
    /// <summary>
    /// Gets or sets the data associated with the response.
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public object? Data { get; set; }
    
    /// <summary>
    /// Gets or sets the collection of error messages associated with the response.
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public IEnumerable<string> Errors { get; set; } = null!;

    /// <summary>
    /// Retrieves the default message for a given HTTP status code.
    /// </summary>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <returns>A default message for the status code.</returns>
    private static string GetDefaultMessageForStatusCode(int statusCode)
    {
        return statusCode switch
        {
            StatusCodes.Status200OK => "Success",
            StatusCodes.Status201Created => "Created",
            StatusCodes.Status400BadRequest => "Bad request",
            StatusCodes.Status401Unauthorized => "Unauthorized",
            StatusCodes.Status403Forbidden => "Forbidden",
            StatusCodes.Status404NotFound => "Resource not found",
            StatusCodes.Status500InternalServerError => "An unhandled error occurred",
            _ => string.Empty
        };
    }
}
