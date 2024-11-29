using System.Data;
using Domain.Interfaces.Command;
using Domain.Interfaces.Query;
using Microsoft.EntityFrameworkCore.Storage;

namespace Domain.Interfaces;

/// <summary>
/// Defines the contract for a unit of work that manages database operations and caching.
/// </summary>
/// <remarks>
/// The Unit of Work pattern coordinates the work of multiple repositories and handles transactions. It also integrates
/// with caching services to optimize data access. This interface provides access to various repositories and methods
/// for managing database transactions and caching.
/// </remarks>
public interface IUnitOfWork : IDisposable
{
    IExecutionStrategy CreateExecutionStrategy();
    /// <summary>
    /// Commits the current transaction to the database asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains the number of affected rows.</returns>
    Task<int> CommitAsync();
    /// <summary>
    /// Begins a new database transaction asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains the transaction object.</returns>
    Task BeginTransactionAsync();
    /// <summary>
    /// Ends the current database transaction asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task EndTransactionAsync();

    Task CommitTransactionAsync();
    /// <summary>
    /// Rolls back the current database transaction asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RollbackTransactionAsync();

    // Task DisposeAsync(IDbContextTransaction transaction);
    #region Command Repository Properties

    IUserAccountCommandRepository UserAccountCommandRepository { get; set; }
    IUserLoginDataCommandRepository UserLoginDataCommandRepository { get; set; }
    IUserTokenCommandRepository UserTokenCommandRepository { get; set; }

    #endregion

    #region Query Repository Properties

    IUserAccountQueryRepository UserAccountQueryRepository { get; set;  }
    IUserLoginDataQueryRepository UserLoginDataQueryRepository { get; set; }
    IUserTokenQueryRepository UserTokenQueryRepository { get; set; }
    
    #endregion
}
