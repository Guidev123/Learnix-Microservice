namespace Learnix.Commons.Infrastructure.Inbox.Models
{
    public sealed class InboxOptions
    {
        public int IntervalInSeconds { get; init; }
        public int BatchSize { get; init; }
    }
}