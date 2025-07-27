namespace Learning.Infrastructure.Inbox
{
    internal sealed record InboxMessageResponse(
        Guid CorrelationId,
        string Content
        );
}