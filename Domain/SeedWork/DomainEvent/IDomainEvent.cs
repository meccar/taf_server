using MediatR;

namespace Domain.SeedWork.DomainEvent;
public interface IDomainEvent : INotification
{
    Guid Id { get; init; }
}
