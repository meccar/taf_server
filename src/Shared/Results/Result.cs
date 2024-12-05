namespace Shared.Results;

public class Result<T>
{
    public bool Succeeded { get; }
    public T? Value { get; }
    public IReadOnlyCollection<string> Errors { get; }

    private Result(bool succeeded, T? value = default, IEnumerable<string>? errors = null)
    {
        Succeeded = succeeded;
        Value = value;
        Errors = errors?.ToArray() ?? Array.Empty<string>();
    }

    public static Result<T> Success(T value) =>
        new(true, value);

    public static Result<T> Failure(params string[] errors) =>
        new(false, errors: errors);

    // Optional: Match method for easier pattern matching
    public TResult Match<TResult>(
        Func<T, TResult> onSuccess, 
        Func<IReadOnlyCollection<string>, TResult> onFailure)
    {
        return Succeeded 
            ? onSuccess(Value!) 
            : onFailure(Errors);
    }
}

public class Result
{
    public bool Succeeded { get; }
    public IReadOnlyCollection<string> Errors { get; }

    private Result(bool succeeded, IEnumerable<string>? errors = null)
    {
        Succeeded = succeeded;
        Errors = errors?.ToArray() ?? Array.Empty<string>();
    }

    public static Result Success() =>
        new(true);

    public static Result Failure(params string[] errors) =>
        new(false, errors);
}