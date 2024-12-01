using Share.SeedWork;

namespace Share.Dtos.Exceptions;

public class ForbiddenException : HttpResponseException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class without a value.
    /// </summary>
    public ForbiddenException() : base(403, "Forbidden")
    {
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class with a specified value.
    /// </summary>
    /// <param name="value">
    /// An object that provides additional information about the resource that was not found. This value is passed to the base exception class.
    /// </param>
    public ForbiddenException(object value) : base(403, value)
    {
    }
}