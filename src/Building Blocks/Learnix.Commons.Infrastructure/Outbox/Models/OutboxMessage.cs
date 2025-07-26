namespace Learnix.Commons.Infrastructure.Outbox.Models
{
    public sealed class OutboxMessage
    {
        public OutboxMessage(
            Guid correlationId,
            string type,
            string content,
            DateTime occurredOn)
        {
            CorrelationId = correlationId;
            Type = type;
            Content = content;
            OccurredOn = occurredOn;
        }

        private OutboxMessage()
        { }

        public Guid CorrelationId { get; private set; }
        public string Type { get; private set; } = string.Empty;
        public string Content { get; private set; } = string.Empty;
        public DateTime OccurredOn { get; private set; }
        public DateTime? ProcessedOn { get; private set; }
        public string? Error { get; private set; }

        public void SetError(string error)
        {
            Error = error;
        }

        public void SetProcessedOn(DateTime processedOn)
        {
            ProcessedOn = processedOn;
        }
    }
}