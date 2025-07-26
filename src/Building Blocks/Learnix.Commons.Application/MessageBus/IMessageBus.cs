using Learnix.Commons.Application.Messaging;

namespace Learnix.Commons.Application.MessageBus
{
    public interface IMessageBus
    {
        Task ProduceAsync<TIntegrationEvent>(
            string topic,
            TIntegrationEvent integrationEvent,
            CancellationToken cancellationToken = default
            ) where TIntegrationEvent : IIntegrationEvent;

        Task ConsumeAsync<TIntegrationEvent>(
            string topic,
            Func<TIntegrationEvent, Task> onMessage,
            CancellationToken cancellationToken = default
            ) where TIntegrationEvent : IIntegrationEvent;
    }
}