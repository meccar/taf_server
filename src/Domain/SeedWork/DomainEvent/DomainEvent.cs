namespace Domain.SeedWork.DomainEvent;

/// <summary>
/// Represents the base class for domain events in the system.
/// </summary>
/// <remarks>
/// The <see cref="DomainEvent"/> class implements the <see cref="IDomainEvent"/> interface and serves as the base for all domain events
/// in the system. It provides a unique identifier for each event through its <see cref="Id"/> property. This base class can be 
/// inherited to create specific domain events that capture important business events or state changes in the system.
/// </remarks>
public abstract record DomainEvent(Guid Id) : IDomainEvent;
