using Domain.Interfaces;
using Domain.SeedWork.Command;

namespace Application.Queries;

public abstract class TransactionalQueryHandler<TQuery, TQueryResponse> 
    : ICommandHandler<TQuery, TQueryResponse>
    where TQuery : ICommand<TQueryResponse>
{
    protected readonly IUnitOfWork _unitOfWork;

    protected TransactionalQueryHandler(
        IUnitOfWork unitOfWork
    )
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<TQueryResponse> Handle(
        TQuery request,
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
    protected abstract Task<TQueryResponse> ExecuteCoreAsync(TQuery request, CancellationToken cancellationToken);

}