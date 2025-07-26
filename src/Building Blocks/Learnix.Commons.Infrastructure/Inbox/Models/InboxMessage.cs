namespace Learnix.Commons.Infrastructure.Inbox.Models
{
    public sealed class InboxMessage
    {
        public InboxMessage(
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

        private InboxMessage()
        { }

        public Guid CorrelationId { get; }
        public string Type { get; } = string.Empty;
        public string Content { get; } = string.Empty;
        public DateTime OccurredOn { get; }
        public DateTime? ProcessedOn { get; }
        public string? Error { get; }
    }
}