namespace Shared.Enums;

/// <summary>
/// Enumeration for the various HTTP request methods.
/// </summary>
public enum ERequestMethod
{
    /// <summary>
    /// Represents an HTTP GET request, typically used to retrieve data from the server.
    /// </summary>
    GET,

    /// <summary>
    /// Represents an HTTP POST request, commonly used to send data to the server.
    /// </summary>
    POST,

    /// <summary>
    /// Represents an HTTP PUT request, usually used to update or replace data on the server.
    /// </summary>
    PUT,

    /// <summary>
    /// Represents an HTTP DELETE request, typically used to remove data from the server.
    /// </summary>
    DELETE,

    /// <summary>
    /// Represents an HTTP PATCH request, used for partial updates to data on the server.
    /// </summary>
    PATCH
}