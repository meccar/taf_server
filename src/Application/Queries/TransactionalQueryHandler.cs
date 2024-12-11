using Domain.Interfaces;
using Domain.SeedWork.Command;
using Domain.SeedWork.Query;

namespace Application.Queries;

/// <summary>
/// Represents a base class for handling transactional queries.
/// This class ensures that the query handler executes within a transactional context, 
/// providing automatic transaction management (commit and rollback) as needed.
/// </summary>
/// <typeparam name="TQuery">The type of the query being handled.</typeparam>
/// <typeparam name="TQueryResponse">The type of the response returned by the query handler.</typeparam>
public abstract class TransactionalQueryHandler<TQuery, TQueryResponse> 
    : IQueryHandler<TQuery, TQueryResponse>
    where TQuery : IQuery<TQueryResponse>
{
    /// <summary>
    /// The unit of work instance used to interact with the data repositories and manage transactions.
    /// </summary>
    protected readonly IUnitOfWork UnitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionalQueryHandler{TQuery, TQueryResponse}"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work instance used to manage the transaction and interact with repositories.</param>
    protected TransactionalQueryHandler(
        IUnitOfWork unitOfWork
    )
    {
        UnitOfWork = unitOfWork;
    }
    
    /// <summary>
    /// Handles the query, executing it within a transactional context. If the operation succeeds, the transaction is committed. 
    /// If an exception occurs, the transaction is rolled back.
    /// </summary>
    /// <param name="request">The query request to be handled.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
    /// <returns>The response of the query operation.</returns>
    /// <exception cref="Exception">Thrown if an error occurs during the execution of the query, causing the transaction to be rolled back.</exception>
    /// <remarks>
    /// This method ensures that the query is executed inside a transaction. It begins a transaction, executes the core logic of the query,
    /// and commits the transaction if successful. If an error occurs, the transaction is rolled back to ensure consistency.
    /// </remarks>
    public async Task<TQueryResponse> Handle(
        TQuery request,
        CancellationToken cancellationToken
        )
    {
        var strategy = UnitOfWork.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(
            state: request,
            verifySucceeded: null,
            cancellationToken: cancellationToken,
            operation: async (context, state, token) =>
            {
                // Begin a new transaction
                await UnitOfWork.BeginTransactionAsync();
                
                try
                {
                    // Execute the core query logic
                    var result = await ExecuteCoreAsync(request, cancellationToken);

                    // Commit the transaction after successful execution
                    await UnitOfWork.CommitTransactionAsync();

                    return result;
                }
                catch (Exception)
                {
                    // Rollback the transaction in case of an error
                    await UnitOfWork.RollbackTransactionAsync();
                    throw;
                }
            });
    }
    
    /// <summary>
    /// Executes the core logic of handling the query. This method should be implemented in the derived class to define the specific query logic.
    /// </summary>
    /// <param name="request">The query request to be handled.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
    /// <returns>The response of the query operation.</returns>
    protected abstract Task<TQueryResponse> ExecuteCoreAsync(TQuery request, CancellationToken cancellationToken);

}