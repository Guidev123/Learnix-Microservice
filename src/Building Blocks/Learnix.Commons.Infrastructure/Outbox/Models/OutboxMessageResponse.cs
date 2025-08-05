namespace Learnix.Commons.Infrastructure.Outbox.Models
{
    public sealed record OutboxMessageResponse(
        Guid CorrelationId,
        string Content
        );
}