using Domain.Interfaces;

namespace Domain.SeedWork.Command;

public abstract class TransactionalCommandHandler<TCommand, TCommandResponse> 
    : ICommandHandler<TCommand, TCommandResponse>
    where TCommand : ICommand<TCommandResponse>
{
    protected readonly IUnitOfWork _unitOfWork;

    protected TransactionalCommandHandler(
        IUnitOfWork unitOfWork
    )
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<TCommandResponse> Handle(
        TCommand request,
        CancellationToken cancellationToken
        )
    {
        var strategy = _unitOfWork.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(
            state: request,
            verifySucceeded: null,
            cancellationToken: cancellationToken,
            operation: async (context, state, token) =>
            {
                await _unitOfWork.BeginTransactionAsync();
                
                try
                {
                    var result = await ExecuteCoreAsync(request, cancellationToken);

                    await _unitOfWork.CommitTransactionAsync();

                    return result;
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            });
    }
    protected abstract Task<TCommandResponse> ExecuteCoreAsync(TCommand request, CancellationToken cancellationToken);

}