using MediatR;

namespace Domain.SeedWork.Query;
/// <summary>
/// Represents a query that performs an action and returns a result of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the result that the query returns.</typeparam>
/// <remarks>
/// This interface inherits from <see cref="IRequest"/>, indicating that it encapsulates a query
/// which executes an operation and returns a result of type <typeparamref name="T"/>. Queries are
/// typically used to perform actions that may modify the state of the system and are intended for
/// operations rather than queries.
/// </remarks>
public interface IQuery<out T> : IRequest<T>;

/// <summary>
/// Represents a query that performs an action and no returns.
/// </summary>
public interface IQuery : IRequest;