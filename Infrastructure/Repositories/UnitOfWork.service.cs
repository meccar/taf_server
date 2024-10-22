using taf_server.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using taf_server.Domain.Interfaces;
using taf_server.Infrastructure.Data;

namespace taf_server.Infrastructure.Repositories;

/// <summary>
/// Represents a unit of work for managing database operations across multiple repositories.
/// </summary>
/// <remarks>
/// This class implements the <see cref="IUnitOfWork"/> interface, providing a way to group multiple 
/// database operations into a single transaction. It allows for committing or rolling back changes 
/// made to the database, ensuring data integrity. The class encapsulates repositories for user accounts 
/// and user login data, providing a cohesive interface for managing related data operations.
/// </remarks>
public class UnitOfWork(
    ApplicationDbContext context,
    bool disposed,
    IUserAccountCommandRepository userAccountCommandRepository,
    IUserLoginDataCommandRepository userLoginDataCommandRepository)
    : IUnitOfWork
{
    private bool _disposed = disposed;

    /// <summary>
    /// Gets the repository for user account commands.
    /// </summary>
    public IUserAccountCommandRepository UserAccountCommandRepository { get; } = userAccountCommandRepository;
    
    /// <summary>
    /// Gets the repository for user login data commands.
    /// </summary>
    public IUserLoginDataCommandRepository UserLoginDataCommandRepository { get; } = userLoginDataCommandRepository;

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
        return context.SaveChangesAsync();
    }

    /// <summary>
    /// Begins a new transaction for the current unit of work.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, with the task result containing the database transaction.</returns>
    public Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return context.Database.BeginTransactionAsync();
    }

    /// <summary>
    /// Commits the current transaction and saves changes to the database.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task EndTransactionAsync()
    {
        await CommitAsync();
        await context.Database.CommitTransactionAsync();
    }

    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task RollbackTransactionAsync()
    {
        return context.Database.RollbackTransactionAsync();
    }

    /// <summary>
    /// Disposes the resources used by the unit of work.
    /// </summary>
    /// <param name="disposing">A value indicating whether the method has been called directly or by the runtime.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
            if (disposing)
                context.Dispose();
        _disposed = true;
    }
}