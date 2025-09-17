using Confluent.Kafka;
using Learnix.Commons.Application.MessageBus;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Infrastructure.Extensions;
using Microsoft.Extensions.Options;

namespace Learnix.Commons.Infrastructure.MessageBus
{
    internal sealed class MessageBus : IMessageBus
    {
        private readonly MessageBusOptions _messageBusOptions;

        public MessageBus(IOptions<MessageBusOptions> options)
        {
            _messageBusOptions = options.Value;
        }

        public async Task ProduceAsync<TIntegrationEvent>(string topic, TIntegrationEvent integrationEvent, CancellationToken cancellationToken = default) where TIntegrationEvent : IIntegrationEvent
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _messageBusOptions.BootstrapServer
            };

            using var producerBuilder = new ProducerBuilder<string, TIntegrationEvent>(config)
                .SetValueSerializer(new KafkaSerializerExtensions<TIntegrationEvent>())
                .Build();

            await producerBuilder.ProduceAsync(topic, new Message<string, TIntegrationEvent>
            {
                Key = integrationEvent.CorrelationId.ToString(),
                Value = integrationEvent
            }, cancellationToken).ConfigureAwait(false);
        }

        public Task ConsumeAsync<TIntegrationEvent>(string topic, Func<TIntegrationEvent, Task> onMessage, CancellationToken cancellationToken = default)
            where TIntegrationEvent : IIntegrationEvent
        {
            return Task.Run(async () =>
            {
                var config = new ConsumerConfig
                {
                    GroupId = _messageBusOptions.GroupId,
                    BootstrapServers = _messageBusOptions.BootstrapServer,
                    EnableAutoCommit = false,
                    EnablePartitionEof = true
                };

                using var consumerBuilder = new ConsumerBuilder<string, TIntegrationEvent>(config)
                    .SetValueDeserializer(new KafkaDeserializerExtensions<TIntegrationEvent>())
                    .Build();

                consumerBuilder.Subscribe(topic);

                while (!cancellationToken.IsCancellationRequested)
                {
                    var result = consumerBuilder.Consume(cancellationToken);
                    if (result.IsPartitionEOF)
                    {
                        continue;
                    }

                    await onMessage(result.Message.Value);

                    consumerBuilder.Commit(result);
                }
            }, cancellationToken);
        }
    }
}