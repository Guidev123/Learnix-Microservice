namespace Learnix.Commons.Application.Messaging
{
    public abstract record IntegrationEvent : IIntegrationEvent
    {
        protected IntegrationEvent()
        {
            CorrelationId = Guid.NewGuid();
            OccurredOn = DateTime.UtcNow;
            Messagetype = GetType().Name;
        }

        public Guid CorrelationId { get; }
        public DateTime OccurredOn { get; }
        public string Messagetype { get; } = null!;
    }
}