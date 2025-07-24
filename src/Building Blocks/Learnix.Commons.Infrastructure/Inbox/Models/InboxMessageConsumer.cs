namespace Learnix.Commons.Infrastructure.Inbox.Models
{
    public sealed class InboxMessageConsumer
    {
        public InboxMessageConsumer(
            Guid inboxMessageCorrelationId,
            string name)
        {
            InboxMessageCorrelationId = inboxMessageCorrelationId;
            Name = name;
        }

        private InboxMessageConsumer()
        { }

        public Guid InboxMessageCorrelationId { get; }
        public string Name { get; } = null!;
    }
}