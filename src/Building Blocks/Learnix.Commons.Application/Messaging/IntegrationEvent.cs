namespace Learnix.Commons.Application.Messaging
{
    public abstract record IntegrationEvent : IIntegrationEvent
    {
        public Guid CorrelationId { get; init; } = Guid.NewGuid();
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
        public string Messagetype { get; init; } = null!;
    }
}