namespace Learnix.Commons.Domain.DomainEvents
{
    public abstract record DomainEvent : IDomainEvent
    {
        protected DomainEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
            CorrelationId = Guid.NewGuid();
            OccurredOn = DateTime.UtcNow;
            Messagetype = GetType().Name;
        }

        public Guid AggregateId { get; }

        public Guid CorrelationId { get; }

        public DateTime OccurredOn { get; }

        public string Messagetype { get; } = string.Empty;
    }
}