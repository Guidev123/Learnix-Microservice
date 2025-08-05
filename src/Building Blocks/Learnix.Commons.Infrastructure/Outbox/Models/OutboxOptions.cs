namespace Learnix.Commons.Infrastructure.Outbox.Models
{
    public sealed class OutboxOptions
    {
        public int IntervalInSeconds { get; init; }
        public int BatchSize { get; init; }
    }
}