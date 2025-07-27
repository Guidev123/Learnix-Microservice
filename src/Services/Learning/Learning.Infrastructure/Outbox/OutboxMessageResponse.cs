namespace Learning.Infrastructure.Outbox
{
    public sealed record OutboxMessageResponse(
        Guid CorrelationId,
        string Content
        );
}