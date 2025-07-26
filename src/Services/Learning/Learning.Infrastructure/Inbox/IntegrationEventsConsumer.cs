using Dapper;
using Learnix.Commons.Application.Factories;
using Learnix.Commons.Application.MessageBus;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Contracts.Users;
using Learnix.Commons.Infrastructure.Extensions;
using Learnix.Commons.Infrastructure.Inbox.Models;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Learning.Infrastructure.Inbox
{
    internal sealed class IntegrationEventsConsumer(IMessageBus messageBus, ISqlConnectionFactory sqlConnectionFactory) : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            messageBus.ConsumeAsync<UserCreatedIntegrationEvent>("users.user-created", StoreInboxMessageAsync, stoppingToken);

            return Task.CompletedTask;
        }

        public async Task StoreInboxMessageAsync<T>(T integrationEvent) where T : IntegrationEvent
        {
            using var connection = sqlConnectionFactory.Create();

            var inboxMessage = new InboxMessage(
                integrationEvent.CorrelationId,
                integrationEvent.GetType().Name,
                JsonConvert.SerializeObject(integrationEvent, SerializerExtensions.Instance),
                integrationEvent.OccurredOn);

            const string sql = """
                INSERT INTO learning.InboxMessages (CorrelationId, Type, Content, OccurredOn)
                VALUES (@CorrelationId, @Type, @Content, @OccurredOn)
                """;

            await connection.ExecuteAsync(sql, inboxMessage);
        }
    }
}