using Domain.Interfaces;
using Domain.Interfaces.Credentials;
using Domain.Interfaces.News;
using Domain.Interfaces.User;
using Microsoft.EntityFrameworkCore.Storage;
using Persistance.Data;

namespace Persistance.Repositories;

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
    /// <param name="userAccountRepository">The repository for user account commands.</param>
    /// <param name="userProfileRepository">The repository for user profile commands.</param>
    /// <param name="userTokenRepository">The repository for user token commands.</param>
    public UnitOfWork(
        ApplicationDbContext context,
        
        IUserAccountRepository userAccountRepository,
        IUserProfileRepository userProfileRepository,
        IUserTokenRepository userTokenRepository,
        
        INewsRepository newsRepository 
        )
    {
        _context = context;

        UserAccountRepository = userAccountRepository;
        UserProfileRepository = userProfileRepository;
        UserTokenRepository = userTokenRepository;

        NewsRepository = newsRepository;
    }
    /// <summary>
    /// Gets the repository for user account commands.
    /// </summary>
    public IUserAccountRepository UserAccountRepository { get; set; }
    
    /// <summary>
    /// Gets the repository for user profile commands.
    /// </summary>
    public IUserProfileRepository UserProfileRepository { get; set; }
    
    /// <summary>
    /// Gets the repository for user token commands.
    /// </summary>
    public IUserTokenRepository UserTokenRepository { get; set; }
    
    /// <summary>
    /// Gets the repository for news commands.
    /// </summary>
    public INewsRepository NewsRepository { get; set; }

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

    /// <summary>
    /// Creates a new execution strategy for the database.
    /// </summary>
    /// <returns>An <see cref="IExecutionStrategy"/> that can be used to execute database operations with a strategy.</returns>
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

    /// <summary>
    /// Commits the current transaction and saves changes to the database.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task CommitTransactionAsync()
    {
        await CommitAsync();
        await _context.Database.CommitTransactionAsync();
    }
    
    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task RollbackTransactionAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }
    
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