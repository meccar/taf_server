using Domain.Interfaces;
using Domain.SeedWork.Command;

namespace Application.Commands;

/// <summary>
/// An abstract base class that handles commands with transactional operations.
/// Provides a common pattern for handling commands that require database transactions,
/// ensuring that the command is executed within a transaction scope with commit/rollback handling.
/// </summary>
/// <typeparam name="TCommand">The type of the command being handled.</typeparam>
/// <typeparam name="TCommandResponse">The type of the response returned after executing the command.</typeparam>
public abstract class TransactionalCommandHandler<TCommand, TCommandResponse> 
    : ICommandHandler<TCommand, TCommandResponse>
    where TCommand : ICommand<TCommandResponse>
{
    /// <summary>
    /// The unit of work instance used to interact with the underlying data store and manage transactions.
    /// </summary>
    protected readonly IUnitOfWork UnitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionalCommandHandler{TCommand, TCommandResponse}"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work used to manage transactions and interact with repositories.</param>
    protected TransactionalCommandHandler(
        IUnitOfWork unitOfWork
    )
    {
        UnitOfWork = unitOfWork;
    }
    /// <summary>
    /// Handles the execution of the given command within a transaction.
    /// It begins a transaction, executes the core logic of the command, and commits the transaction.
    /// If an error occurs, the transaction is rolled back.
    /// </summary>
    /// <param name="request">The command to be executed.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the asynchronous operation to complete.</param>
    /// <returns>A <see cref="Task{TCommandResponse}"/> representing the asynchronous operation, with the command's response as the result.</returns>
    /// <exception cref="Exception">Thrown if an error occurs during the command execution. The transaction will be rolled back.</exception>
    public async Task<TCommandResponse> Handle(
        TCommand request,
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
                await UnitOfWork.BeginTransactionAsync();
                
                try
                {
                    var result = await ExecuteCoreAsync(request, cancellationToken);

                    await UnitOfWork.CommitTransactionAsync();

                    return result;
                }
                catch (Exception)
                {
                    await UnitOfWork.RollbackTransactionAsync();
                    throw;
                }
            });
    }
    
    /// <summary>
    /// Contains the core logic for executing the command.
    /// This method must be implemented in derived classes to define the specific behavior of the command.
    /// </summary>
    /// <param name="request">The command request containing the data necessary for execution.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the asynchronous operation to complete.</param>
    /// <returns>A <see cref="Task{TCommandResponse}"/> representing the asynchronous operation, with the command's response as the result.</returns>
    protected abstract Task<TCommandResponse> ExecuteCoreAsync(TCommand request, CancellationToken cancellationToken);

}