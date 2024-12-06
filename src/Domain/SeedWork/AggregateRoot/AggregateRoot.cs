using Domain.Entities;
using Domain.SeedWork.DomainEvent;

namespace Domain.SeedWork.AggregateRoot;

/// <summary>
/// Represents the base class for aggregate roots in the domain layer.
/// </summary>
/// <remarks>
/// The AggregateRoot class extends from <see cref="EntityBase"/> and serves as a base for domain aggregates.
/// It provides mechanisms to track and raise domain events, which are crucial for maintaining consistency
/// and triggering necessary side-effects in the application.
/// </remarks>
public abstract class AggregateRoot : EntityBase
{
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateRoot"/> class.
    /// </summary>
    protected AggregateRoot()
    {
    }

    /// <summary>
    /// Gets the list of domain events that have been raised for this aggregate root.
    /// </summary>
    /// <returns>A read-only collection of domain events.</returns>
    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();

    /// <summary>
    /// Adds a new domain event to the aggregate root.
    /// </summary>
    /// <param name="domainEvent">The domain event to be added.</param>
    /// <remarks>
    /// This method is protected so that it can only be called from within the aggregate root class or its derived classes.
    /// Domain events can represent significant occurrences in the lifecycle of an aggregate that might require external reactions.
    /// </remarks>
    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    /// <summary>
    /// Clears all domain events that have been raised for this aggregate root.
    /// </summary>
    /// <remarks>
    /// This method is typically used after domain events have been processed and we no longer need to track them.
    /// </remarks>
    public void ClearDomainEvents() => _domainEvents.Clear();

    /// <summary>
    /// Raises a new domain event for this aggregate root.
    /// </summary>
    /// <param name="domainEvent">The domain event to be raised.</param>
    /// <remarks>
    /// This method is typically called to signal that something important has happened in the aggregate root that other parts
    /// of the system should react to. It is essentially an alias for adding the domain event to the internal list of events.
    /// </remarks>
    protected void RaiseDomainEvent(IDomainEvent domainEvent) =>
        _domainEvents.Add(domainEvent);
}
