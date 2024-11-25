namespace Domain.SeedWork.DomainEvent;
public abstract record DomainEvent(Guid Id) : IDomainEvent;
