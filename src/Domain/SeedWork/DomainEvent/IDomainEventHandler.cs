using MediatR;

namespace Domain.SeedWork.DomainEvent;

/// <summary>
/// Represents a handler for domain events.
/// </summary>
/// <typeparam name="TEvent">The type of the domain event that the handler is responsible for handling.</typeparam>
/// <remarks>
/// The <see cref="IDomainEventHandler{TEvent}"/> interface is used to define handlers for domain events. A domain event handler
/// listens for and processes specific domain events, typically to perform actions or side effects in response to the event.
/// </remarks>
public interface IDomainEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : IDomainEvent
{
}