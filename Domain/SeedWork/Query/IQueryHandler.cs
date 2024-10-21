using MediatR;

namespace taf_server.Domain.SeedWork.Query;

/// <summary>
/// Defines a handler for processing queries that return a result.
/// </summary>
/// <typeparam name="TQuery">The type of the query to handle. Must implement <see cref="IQuery{TQueryResponse}"/>.</typeparam>
/// <typeparam name="TQueryResponse">The type of the response returned by the query handler.</typeparam>
/// <remarks>
/// This interface inherits from <see cref="IRequestHandler{TRequest}"/>, which means it is used
/// to handle queries of type <typeparamref name="TQuery"/> that produce a result of type <typeparamref name="TQueryResponse"/>.
/// Implementations of this interface are responsible for executing the query and returning a result based on the query's logic.
/// </remarks>
public interface IQueryHandler<TQuery, TQueryResponse> : IRequestHandler<TQuery, TQueryResponse>
    where TQuery : IQuery<TQueryResponse>
{
}

/// <summary>
/// Defines a handler for processing queries that do not return a result.
/// </summary>
/// <typeparam name="TQuery">The type of the query to handle. Must implement <see cref="IQuery"/>.</typeparam>
/// <remarks>
/// This interface inherits from <see cref="IRequestHandler{TQuery}"/>, which indicates it is used
/// to handle queries of type <typeparamref name="TQuery"/> that do not return a result. Implementations of this
/// interface are responsible for executing the query's logic without producing a result.
/// </remarks>
public interface IQueryHandler<TQuery> : IRequestHandler<TQuery>
    where TQuery : IQuery
{
}