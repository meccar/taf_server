// using System.Data;
// using System.Runtime.CompilerServices;
// using Infrastructure.Data;
// using Infrastructure.Extensions;
// using Microsoft.EntityFrameworkCore;
//
// namespace Infrastructure.Decorators;
//
// public class TransactionOptions
// {
//     public IsolationLevel IsolationLevel { get; set; } = IsolationLevel.ReadCommitted;
//     public bool Replication { get; set; }
//     public string ConnectionName { get; set; } = "Default";
// }
//
// public class TransactionContext
// {
//     private static readonly AsyncLocal<TransactionContext> Current = new();
//     private readonly Dictionary<string, DbContext> _contexts = new();
//
//     public static TransactionContext GetCurrent() => Current.Value ??= new TransactionContext();
//
//     public void SetContext(string key, DbContext context) => _contexts[key] = context;
//     
//     public DbContext GetContext(string key) => _contexts.TryGetValue(key, out var context) ? context : null;
// }
//
// [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
// public class TransactionalAttribute : Attribute
// {
//     public IsolationLevel IsolationLevel { get; set; } = IsolationLevel.ReadCommitted;
//     public bool Replication { get; set; }
//     public string ConnectionName { get; set; } = "Default";
//
//     public TransactionalAttribute(
//         IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, 
//         bool replication = false, 
//         string connectionName = "Default")
//     {
//         IsolationLevel = isolationLevel;
//         Replication = replication;
//         ConnectionName = connectionName;
//     }
//     
//     public TransactionalAttribute(TransactionOptions options)
//     {
//         if (options != null)
//         {
//             IsolationLevel = options.IsolationLevel;
//             Replication = options.Replication;
//             ConnectionName = options.ConnectionName;
//         }
//     }
// }
//
// public class TransactionDecorator
// {
//     private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
//
//     public TransactionDecorator(IDbContextFactory<ApplicationDbContext> contextFactory)
//     {
//         _contextFactory = contextFactory;
//     }
//
//     public async Task<T> WrapInTransaction<T>(
//         Func<Task<T>> method,
//         TransactionOptions options = null,
//         [CallerMemberName] string methodName = null)
//     {
//         options ??= new TransactionOptions();
//         var context = await _contextFactory.CreateDbContextAsync();
//         var transactionContext = TransactionContext.GetCurrent();
//         transactionContext.SetContext("typeOrmEntityManager", context);
//
//         var strategy = context.Database.CreateExecutionStrategy();
//
//         return await strategy.ExecuteAsync(async () =>
//         {
//             using var transaction = await context.Database.BeginTransactionAsync(
//                 options.IsolationLevel);
//
//             try
//             {
//                 if (options.Replication)
//                 {
//                     await context.Database.UseReplicationAsync();
//                 }
//
//                 var result = await method();
//                 await transaction.CommitAsync();
//                 return result;
//             }
//             catch
//             {
//                 await transaction.RollbackAsync();
//                 throw;
//             }
//             finally
//             {
//                 await context.DisposeAsync();
//             }
//         });
//     }
// }
//
// public static class TransactionExtensions
// {
//     public static async Task<T> ExecuteTransactional<T>(
//         this object instance,
//         Func<Task<T>> method,
//         TransactionOptions options = null)
//     {
//         var interceptor = GetInterceptor(instance);
//         return await interceptor.WrapInTransaction(method, options);
//     }
//
//     private static TransactionDecorator GetInterceptor(object instance)
//     {
//         throw new NotImplementedException();
//     }
// }

using System.Data;
using System.Reflection;
using Domain.Interfaces;
using Domain.SeedWork.Command;
using Domain.SeedWork.Transactions;
using Microsoft.Extensions.Logging;

public abstract class TransactionalDecorator<TCommand, TCommandResponse> 
    : ICommandHandler<TCommand, TCommandResponse>
    where TCommand : ICommand<TCommandResponse>
{
    protected readonly IUnitOfWork _unitOfWork;

    protected TransactionalDecorator(
        IUnitOfWork unitOfWork
    )
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<TCommandResponse> Handle(TCommand request, CancellationToken cancellationToken)
    {
        var strategy = _unitOfWork.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(
            state: request,
            verifySucceeded: null,
            cancellationToken: cancellationToken,
            operation: async (context, state, token) =>
            {
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