namespace Learnix.Commons.Domain.DomainEvents
{
    public abstract record DomainEvent : IDomainEvent
    {
        public Guid AggregateId { get; init; }

        public Guid CorrelationId { get; init; } = Guid.NewGuid();

        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;

        public string Messagetype { get; init; } = null!;
    }
}