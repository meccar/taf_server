namespace Shared.Results;

/// <summary>
/// Represents the result of an operation, with a value of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the value returned in case of a successful result.</typeparam>
public class Result<T>
{
    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    public bool Succeeded { get; }
    
    /// <summary>
    /// Gets the value returned by the operation if successful.
    /// </summary>
    public T? Value { get; }
    
    /// <summary>
    /// Gets the collection of error messages if the operation failed.
    /// </summary>
    public IReadOnlyCollection<string> Errors { get; }

    private Result(bool succeeded, T? value = default, IEnumerable<string>? errors = null)
    {
        Succeeded = succeeded;
        Value = value;
        Errors = errors?.ToArray() ?? Array.Empty<string>();
    }

    /// <summary>
    /// Creates a successful result with the specified value.
    /// </summary>
    /// <param name="value">The value to return in case of success.</param>
    /// <returns>A <see cref="Result{T}"/> indicating success.</returns>
    public static Result<T> Success(T value) =>
        new(true, value);

    /// <summary>
    /// Creates a failed result with the specified error messages.
    /// </summary>
    /// <param name="errors">The error messages to include in the result.</param>
    /// <returns>A <see cref="Result{T}"/> indicating failure.</returns>
    public static Result<T> Failure(params string[] errors) =>
        new(false, errors: errors);

    // Optional: Match method for easier pattern matching
    
    /// <summary>
    /// Matches the result with corresponding handlers for success or failure.
    /// </summary>
    /// <typeparam name="TResult">The type of the result of the matching operation.</typeparam>
    /// <param name="onSuccess">The function to handle the success case.</param>
    /// <param name="onFailure">The function to handle the failure case.</param>
    /// <returns>The result of the matching operation.</returns>
    public TResult Match<TResult>(
        Func<T, TResult> onSuccess, 
        Func<IReadOnlyCollection<string>, TResult> onFailure)
    {
        return Succeeded 
            ? onSuccess(Value!) 
            : onFailure(Errors);
    }
}

/// <summary>
/// Represents the result of an operation that does not return a value.
/// </summary>
public class Result
{
    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    public bool Succeeded { get; }
    
    /// <summary>
    /// Gets the collection of error messages if the operation failed.
    /// </summary>
    public IReadOnlyCollection<string> Errors { get; }

    private Result(bool succeeded, IEnumerable<string>? errors = null)
    {
        Succeeded = succeeded;
        Errors = errors?.ToArray() ?? Array.Empty<string>();
    }

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    /// <returns>A <see cref="Result"/> indicating success.</returns>
    public static Result Success() =>
        new(true);

    /// <summary>
    /// Creates a failed result with the specified error messages.
    /// </summary>
    /// <param name="errors">The error messages to include in the result.</param>
    /// <returns>A <see cref="Result"/> indicating failure.</returns>
    public static Result Failure(params string[] errors) =>
        new(false, errors);
}