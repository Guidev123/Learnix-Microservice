namespace Learnix.Commons.Application.Messaging
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent : IIntegrationEvent
    {
        Task ExecuteAsync(TIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
    }

    public interface IIntegrationEventHandler
    {
        Task ExecuteAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
    }
}