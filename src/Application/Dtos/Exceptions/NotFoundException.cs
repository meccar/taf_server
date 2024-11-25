using Application.SeedWork;

namespace Application.Dtos.Exceptions;

/// <summary>
/// Represents an exception that is thrown when a requested resource is not found (HTTP 404).
/// </summary>
public class NotFoundException : HttpResponseException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class without a value.
    /// </summary>
    public NotFoundException() : base(404, "Resource not found")
    {
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class with a specified value.
    /// </summary>
    /// <param name="value">
    /// An object that provides additional information about the resource that was not found. This value is passed to the base exception class.
    /// </param>
    public NotFoundException(object value) : base(404, value)
    {
    }
}