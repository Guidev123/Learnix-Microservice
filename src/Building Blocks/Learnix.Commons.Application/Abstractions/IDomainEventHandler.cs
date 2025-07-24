using Learnix.Commons.Domain.DomainEvents;

namespace Learnix.Commons.Application.Abstractions
{
    public interface IDomainEventHandler<in TDomainEvent> : IDomainEventHandler
        where TDomainEvent : IDomainEvent
    {
        Task ExecuteAsync(TDomainEvent domainEvent, CancellationToken cancellationToken = default);
    }

    public interface IDomainEventHandler
    {
        Task ExecuteAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
    }
}