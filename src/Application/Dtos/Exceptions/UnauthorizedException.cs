
using Share.SeedWork;

namespace Application.Dtos.Exceptions;

/// <summary>
/// Represents an exception that is thrown when an unauthorized request (HTTP 401) occurs.
/// </summary>
public class UnauthorizedException : HttpResponseException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnauthorizedException"/> class without a value.
    /// </summary>
    public UnauthorizedException() : base(401, "Unauthorized request")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnauthorizedException"/> class with a specified value.
    /// </summary>
    /// <param name="value">
    /// An object that provides additional information about the unauthorized request. This value is passed to the base exception class.
    /// </param>
    public UnauthorizedException(object value) : base(401, value)
    {
    }
}