using taf_server.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using taf_server.Domain.Interfaces;
using taf_server.Infrastructure.Data;

namespace taf_server.Infrastructure.Repositories;
public class UnitOfWork(
    ApplicationDbContext context,
    bool disposed,
    IUserAccountCommandRepository userAccountCommandRepository,
    IUserLoginDataCommandRepository userLoginDataCommandRepository)
    : IUnitOfWork
{
    private bool _disposed = disposed;

    public IUserAccountCommandRepository UserAccountCommandRepository { get; } = userAccountCommandRepository;
    public IUserLoginDataCommandRepository UserLoginDataCommandRepository { get; } = userLoginDataCommandRepository;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public Task<int> CommitAsync()
    {
        return context.SaveChangesAsync();
    }

    public Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return context.Database.BeginTransactionAsync();
    }

    public async Task EndTransactionAsync()
    {
        await CommitAsync();
        await context.Database.CommitTransactionAsync();
    }

    public Task RollbackTransactionAsync()
    {
        return context.Database.RollbackTransactionAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
            if (disposing)
                context.Dispose();
        _disposed = true;
    }
}