namespace taf_server.Application.Exceptions;

/// <summary>
/// Represents an exception that can be thrown to return a specific HTTP response status code and optional value.
/// </summary>
/// <remarks>
/// This class is used to signal that an HTTP response should be generated with a specific status code and an optional value.
/// It is useful for handling and customizing HTTP responses in a web application.
/// </remarks>
public class HttpResponseException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpResponseException"/> class with a specified status code and optional value.
    /// </summary>
    /// <param name="statusCode">The HTTP status code to be returned.</param>
    /// <param name="value">An optional value to include in the response. This can be any object, such as an error message or additional data.</param>
    public HttpResponseException(int statusCode, object? value = null)
        : base(value?.ToString() ?? string.Empty)
    {
        StatusCode = statusCode;
        // Message = value?.ToString() ?? "";
    }

    /// <summary>
    /// Gets the HTTP status code associated with the exception.
    /// </summary>
    /// <value>
    /// An integer representing the HTTP status code. This is used to indicate the type of error or response.
    /// </value>
    public int StatusCode { get; }

    /// <summary>
    /// Gets a new message that provides additional context.
    /// </summary>
    public new string Message => base.Message;
}
