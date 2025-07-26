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