using Learnix.Commons.Domain.DomainEvents;

namespace Learnix.Commons.Application.Abstractions
{
    public abstract class DomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
        public abstract Task ExecuteAsync(TDomainEvent domainEvent, CancellationToken cancellationToken = default);

        public Task ExecuteAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            return ExecuteAsync((TDomainEvent)domainEvent, cancellationToken);
        }
    }
}