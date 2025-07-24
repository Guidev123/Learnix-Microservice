namespace Learnix.Commons.Infrastructure.Outbox.Models
{
    public sealed class OutboxMessageConsumer
    {
        public OutboxMessageConsumer(
            Guid outboxMessageCorrelationId,
            string name)
        {
            OutboxMessageCorrelationId = outboxMessageCorrelationId;
            Name = name;
        }

        private OutboxMessageConsumer()
        { }

        public Guid OutboxMessageCorrelationId { get; }
        public string Name { get; } = null!;
    }
}