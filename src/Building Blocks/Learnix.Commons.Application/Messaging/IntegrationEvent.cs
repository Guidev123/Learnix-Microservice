namespace Learnix.Commons.Application.Messaging
{
    public abstract record IntegrationEvent : IIntegrationEvent
    {
        public Guid CorrelationId { get; init; }
        public DateTime OccurredOn { get; init; }
        public string Messagetype { get; init; } = null!;
    }
}