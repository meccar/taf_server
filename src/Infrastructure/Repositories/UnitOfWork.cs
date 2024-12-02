using DataBase.Data;
using Domain.Interfaces;
using Domain.Interfaces.Command;
using Domain.Interfaces.Query;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Repositories;

/// <summary>
/// Represents a unit of work for managing database operations across multiple repositories.
/// </summary>
/// <remarks>
/// This class implements the <see cref="IUnitOfWork"/> interface, providing a way to group multiple 
/// database operations into a single transaction. It allows for committing or rolling back changes 
/// made to the database, ensuring data integrity. The class encapsulates repositories for user accounts 
/// and user login data, providing a cohesive interface for managing related data operations.
/// </remarks>
public class UnitOfWork : IUnitOfWork
{
    // private readonly IDbConnection _connection;
    private readonly ApplicationDbContext _context;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="context">The database context used for data operations.</param>
    /// 
    /// <param name="userAccountCommandRepository">The repository for user account commands.</param>
    /// <param name="userLoginDataCommandRepository">The repository for user login data commands.</param>
    /// 
    /// <param name="userAccountQueryRepository">The repository for user account queries.</param>
    /// <param name="userLoginDataQueryRepository">The repository for user login data queries.</param>
    /// 
    public UnitOfWork(
        ApplicationDbContext context,
        // IDbConnection connection,
        
        IUserAccountRepository userAccountRepository,
        IUserProfileRepository userProfileRepository,
        IUserTokenRepository userTokenRepository
        )
    {
        _context = context;
        // _connection = connection;
        
        UserAccountRepository = userAccountRepository;
        UserProfileRepository = userProfileRepository;
        UserTokenRepository = userTokenRepository;
    }
    public IUserAccountRepository UserAccountRepository { get; set; }
    public IUserProfileRepository UserProfileRepository { get; set; }
    public IUserTokenRepository UserTokenRepository { get; set; }
    
    /// <summary>
    /// Releases the resources used by the <see cref="UnitOfWork"/> class.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Commits all changes made in this unit of work to the database.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, with the task result containing the number of state entries written to the database.</returns>
    public Task<int> CommitAsync()
    {
        return _context.SaveChangesAsync();
    }

    public IExecutionStrategy CreateExecutionStrategy()
    {
        return _context.Database.CreateExecutionStrategy();
    }

    /// <summary>
    /// Begins a new transaction for the current unit of work.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, with the task result containing the database transaction.</returns>
    public async Task BeginTransactionAsync()
    {
        // if (_connection.State != ConnectionState.Open)
        // { 
        //     _connection.Open();
        // }s
        //
        // return await Task.FromResult(_connection.BeginTransaction());
        await _context.Database.BeginTransactionAsync();
    }

    /// <summary>
    /// Commits the current transaction and saves changes to the database.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task EndTransactionAsync()
    {
        await CommitAsync();
        await _context.Database.CommitTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        // transaction.Commit();
        // await Task.CompletedTask;
        await CommitAsync();
        await _context.Database.CommitTransactionAsync();
    }
    
    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task RollbackTransactionAsync()
    {
        // return _context.Database.RollbackTransactionAsync();
        // transaction.Rollback();
        // await Task.CompletedTask;
        await _context.Database.RollbackTransactionAsync();
    }

    // public async Task DisposeAsync()
    // {
    //     // return _context.Database.RollbackTransactionAsync();
    //     // transaction.Dispose();
    //     // await Task.CompletedTask;
    //     return _context.Database.
    // }
    
    /// <summary>
    /// Disposes the resources used by the unit of work.
    /// </summary>
    /// <param name="disposing">A value indicating whether the method has been called directly or by the runtime.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
            if (disposing)
                _context.Dispose();
        _disposed = true;
    }
}