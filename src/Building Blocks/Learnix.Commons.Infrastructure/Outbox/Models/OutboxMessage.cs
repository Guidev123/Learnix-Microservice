namespace Learnix.Commons.Infrastructure.Outbox.Models
{
    public sealed class OutboxMessage
    {
        public OutboxMessage(
            Guid correlationId,
            string type,
            string content,
            DateTime occurredOn,
            DateTime? processedOn,
            string? error)
        {
            CorrelationId = correlationId;
            Type = type;
            Content = content;
            OccurredOn = occurredOn;
            ProcessedOn = processedOn;
            Error = error;
        }

        private OutboxMessage()
        { }

        public Guid CorrelationId { get; }
        public string Type { get; } = string.Empty;
        public string Content { get; } = string.Empty;
        public DateTime OccurredOn { get; }
        public DateTime? ProcessedOn { get; }
        public string? Error { get; }
    }
}