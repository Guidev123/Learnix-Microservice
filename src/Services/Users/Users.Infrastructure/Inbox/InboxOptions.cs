using Learnix.Commons.Infrastructure.Outbox.Models;

namespace Users.Infrastructure.Inbox
{
    internal sealed class InboxOptions
    {
        public int IntervalInSeconds { get; init; }
        public int BatchSize { get; init; }
    }
}