using System.Linq.Expressions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.SeedWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Persistance.Data;

namespace Persistance.Repositories;
/// <summary>
/// Provides a base implementation for repository classes handling entities of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the entity managed by this repository. It must derive from <see cref="EntityBase"/>.</typeparam>
/// <remarks>
/// This base class implements common repository operations such as CRUD operations for entities of type <typeparamref name="T"/>.
/// It is designed to be extended by specific repository classes to provide entity-specific operations.
/// </remarks>
public class RepositoryBase<T> : IRepositoryBase<T> where T : EntityBase
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryBase{T}"/> class.
    /// </summary>
    /// <param name="context">The database context used for database operations.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is <c>null</c>.</exception>
    public RepositoryBase(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    /// <summary>
    /// Retrieves all entities, optionally tracking changes.
    /// </summary>
    /// <param name="trackChanges">Indicates whether to track changes to the entities.</param>
    /// <returns>An <see cref="IQueryable{T}"/> of all entities.</returns>
    public IQueryable<T> FindAll(bool trackChanges = false)
    {
        return !trackChanges
            ? Queryable.Where<T>(_context.Set<T>().AsNoTracking(), e => e.DeletedAt == null)
            : Queryable.Where<T>(_context.Set<T>(), e => e.DeletedAt == null);
    }
    /// <summary>
    /// Retrieves all entities, optionally tracking changes and including specified related entities.
    /// </summary>
    /// <param name="trackChanges">Indicates whether to track changes to the entities.</param>
    /// <param name="includeProperties">The related entities to include.</param>
    /// <returns>An <see cref="IQueryable{T}"/> of all entities with specified includes.</returns>
    public IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
    {
        var items = FindAll(trackChanges);
        items = includeProperties
            .Aggregate(items, (current, includeProperty) =>
                current.Include(includeProperty));
        return items;
    }
    /// <summary>
    /// Finds entities matching the specified condition, optionally tracking changes.
    /// </summary>
    /// <param name="expression">The condition to filter entities.</param>
    /// <param name="trackChanges">Indicates whether to track changes to the entities.</param>
    /// <returns>An <see cref="IQueryable{T}"/> of entities matching the condition.</returns>
    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false)
    {
        return !trackChanges
            ? Queryable.Where<T>(_context.Set<T>(), e => e.DeletedAt == null).Where(expression).AsNoTracking()
            : Queryable.Where<T>(_context.Set<T>(), e => e.DeletedAt == null).Where(expression);
    }
    /// <summary>
    /// Finds entities matching the specified condition, optionally tracking changes and including specified related entities.
    /// </summary>
    /// <param name="expression">The condition to filter entities.</param>
    /// <param name="trackChanges">Indicates whether to track changes to the entities.</param>
    /// <param name="includeProperties">The related entities to include.</param>
    /// <returns>An <see cref="IQueryable{T}"/> of entities matching the condition with specified includes.</returns>
    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false,
        params Expression<Func<T, object>>[] includeProperties)
    {
        var items = FindByCondition(expression, trackChanges);
        items = includeProperties
            .Aggregate(items, (current, includeProperty) =>
                current.Include(includeProperty));
        return items;
    }
    /// <summary>
    /// Checks if an entity with the specified identifier exists asynchronously.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains 
    /// a boolean value indicating whether the entity exists.</returns>
    public async Task<bool> ExistAsync(string id)
    {
        return await _context.Set<T>().AnyAsync(x => new string(x.GetType().GetProperty("Id")!.ToString()!) == id);
    }
    /// <summary>
    /// Checks if any entity matching the specified condition exists asynchronously.
    /// </summary>
    /// <param name="expression">The condition to check for existence.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains 
    /// a boolean value indicating whether any matching entity exists.</returns>
    public async Task<bool> ExistAsync(Expression<Func<T, bool>> expression)
    {
        return await _context.Set<T>().AnyAsync(expression);
    }
    /// <summary>
    /// Retrieves an entity by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains 
    /// the entity with the specified identifier.</returns>
    public async Task<T> GetByIdAsync(string id)
    {
        return (await _context.Set<T>().FindAsync(id))!;
    }
    /// <summary>
    /// Retrieves an entity by its identifier asynchronously, including specified related entities.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
    /// <param name="includeProperties">The related entities to include.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains 
    /// the entity with the specified identifier and included properties.</returns>
    public async Task<T> GetByIdAsync(string id, params Expression<Func<T, object>>[] includeProperties)
    {
        return (await _context.Set<T>().FindAsync(id, includeProperties))!;
    }
    /// <summary>
    /// Creates a new entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to create.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task<EntityEntry<T>?> CreateAsync(T entity)
    {
        var addResult = await _context.Set<T>().AddAsync(entity);
        var result = await _context.SaveChangesAsync();
        return result > 0 ? addResult : null;
    }
    /// <summary>
    /// Creates a list of entities asynchronously.
    /// </summary>
    /// <param name="entities">The list of entities to create.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task CreateListAsync(IEnumerable<T> entities)
    {
        await _context.Set<T>().AddRangeAsync(entities);
    }
    /// <summary>
    /// Updates an existing entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task UpdateAsync(T entity)
    {
        if (_context.Entry(entity).State != EntityState.Unchanged)
            _context.Entry(entity).CurrentValues.SetValues(entity);
        return Task.CompletedTask;
    }
    /// <summary>
    /// Deletes an existing entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }
    /// <summary>
    /// Deletes a list of entities asynchronously.
    /// </summary>
    /// <param name="entities">The list of entities to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task DeleteListAsync(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
        return Task.CompletedTask;
    }
    /// <summary>
    /// Soft deletes an existing entity by setting its deletion timestamp asynchronously.
    /// </summary>
    /// <param name="entity">The entity to soft delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task SoftDeleteAsync(T entity)
    {
        _context.Entry(entity).Property(nameof(ISoftDeletable.DeletedAt)).CurrentValue = DateTime.UtcNow;
        return Task.CompletedTask;
    }
    /// <summary>
    /// Restores a soft-deleted entity by clearing its deletion timestamp asynchronously.
    /// </summary>
    /// <param name="entity">The entity to restore.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task RestoreAsync(T entity)
    {
        _context.Entry(entity).Property(nameof(ISoftDeletable.DeletedAt)).CurrentValue = null;
        return Task.CompletedTask;
    }
}