using MediatR;

namespace Domain.SeedWork.DomainEvent;

/// <summary>
/// Represents a domain event in the system.
/// </summary>
/// <remarks>
/// The <see cref="IDomainEvent"/> interface defines the structure of a domain event that can be handled within the domain layer. 
/// Domain events are typically used to model significant occurrences or state changes in the business logic that may 
/// have consequences for other parts of the system.
/// </remarks>
public interface IDomainEvent : INotification
{
    /// <summary>
    /// Gets the unique identifier for the domain event.
    /// </summary>
    /// <remarks>
    /// The <see cref="Id"/> is used to uniquely identify the domain event within the system. This allows for consistent event 
    /// tracking and management, and is often used in event-sourcing systems where events are replayed to rebuild state.
    /// </remarks>
    Guid Id { get; init; }
}