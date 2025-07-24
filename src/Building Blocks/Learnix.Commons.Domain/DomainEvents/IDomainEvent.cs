using Learnix.Commons.Domain.DomainObjects;

namespace Learnix.Commons.Domain.DomainEvents
{
    public interface IDomainEvent : IEvent
    {
        Guid AggregateId { get; }
    }
}