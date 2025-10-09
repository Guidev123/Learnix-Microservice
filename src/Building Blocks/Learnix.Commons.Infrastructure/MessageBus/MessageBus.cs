using Confluent.Kafka;
using Learnix.Commons.Application.MessageBus;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Learnix.Commons.Infrastructure.MessageBus
{
    internal sealed class MessageBus(
        IOptions<MessageBusOptions> options,
        ILogger<MessageBus> logger
        ) : IMessageBus
    {
        private readonly MessageBusOptions _messageBusOptions = options.Value;

        public async Task ProduceAsync<TIntegrationEvent>(
            string topic,
            TIntegrationEvent integrationEvent,
            CancellationToken cancellationToken = default
            ) where TIntegrationEvent : IIntegrationEvent
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _messageBusOptions.BootstrapServer,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = _messageBusOptions.SaslUsername,
                SaslPassword = _messageBusOptions.SaslPassword
            };

            using var producerBuilder = new ProducerBuilder<string, TIntegrationEvent>(config)
                .SetValueSerializer(new KafkaSerializerExtensions<TIntegrationEvent>())
                .SetErrorHandler((_, e) =>
                {
                    logger.LogError("Kafka producer error: {Error}", e.Reason);
                })
                .Build();

            await producerBuilder.ProduceAsync(topic, new Message<string, TIntegrationEvent>
            {
                Key = integrationEvent.CorrelationId.ToString(),
                Value = integrationEvent
            }, cancellationToken).ConfigureAwait(false);
        }

        public async Task ConsumeAsync<TIntegrationEvent>(
            string topic,
            Func<TIntegrationEvent, Task> onMessage,
            CancellationToken cancellationToken = default
            ) where TIntegrationEvent : IIntegrationEvent
        {
            var config = new ConsumerConfig
            {
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = _messageBusOptions.SaslUsername,
                SaslPassword = _messageBusOptions.SaslPassword,
                GroupId = _messageBusOptions.GroupId,
                BootstrapServers = _messageBusOptions.BootstrapServer,
                EnableAutoCommit = false,
                EnablePartitionEof = true
            };

            using var consumerBuilder = new ConsumerBuilder<string, TIntegrationEvent>(config)
                .SetValueDeserializer(new KafkaDeserializerExtensions<TIntegrationEvent>())
                .SetErrorHandler((_, e) =>
                {
                    logger.LogError("Kafka consumer error: {Error}", e.Reason);
                })
                .Build();

            try
            {
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
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing messages from topic {Topic}", topic);
            }
        }
    }
}