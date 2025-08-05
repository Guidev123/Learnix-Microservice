namespace Learnix.Commons.Infrastructure.Inbox.Models
{
    public sealed record InboxMessageResponse(
        Guid CorrelationId,
        string Content
        );
}