﻿using Dapper;
using Learnix.Commons.Application.Factories;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Infrastructure.Inbox.Models;
using System.Data.Common;

namespace Learning.Infrastructure.Inbox
{
    internal sealed class IdempotentIntegrationEventHandlerDecorator<TIntegrationEvent>(
        IntegrationEventHandler<TIntegrationEvent> innerHandler,
        ISqlConnectionFactory sqlConnectionFactory) : IntegrationEventHandler<TIntegrationEvent>
        where TIntegrationEvent : IntegrationEvent
    {
        public override async Task ExecuteAsync(TIntegrationEvent notification, CancellationToken cancellationToken)
        {
            using var connection = sqlConnectionFactory.Create();

            var inboxMessageConsumer = new InboxMessageConsumer(notification.CorrelationId, notification.Messagetype);

            if (await IsInboxMessageProcessedAsync(inboxMessageConsumer, connection))
            {
                return;
            }

            await innerHandler.ExecuteAsync(notification, cancellationToken);

            await MarkInboxMessageAsProcessedAsync(inboxMessageConsumer, connection);
        }

        private static async Task<bool> IsInboxMessageProcessedAsync(
          InboxMessageConsumer inboxMessageConsumer,
          DbConnection connection)
        {
            var sql = $@"
                SELECT CASE
                    WHEN EXISTS(
                        SELECT 1
                        FROM learning.InboxMessageConsumers
                        WHERE InboxMessageCorrelationId = @InboxMessageCorrelationId
                        AND Name = @Name)
                   THEN 1
                   ELSE 0
                END";

            return await connection.ExecuteScalarAsync<bool>(sql, inboxMessageConsumer);
        }

        private static async Task MarkInboxMessageAsProcessedAsync(
            InboxMessageConsumer inboxMessageConsumer,
            DbConnection connection)
        {
            var sql = $@"
                INSERT INTO learning.InboxMessageConsumers (InboxMessageCorrelationId, Name)
                VALUES (@InboxMessageCorrelationId, @Name)";

            await connection.ExecuteAsync(sql, inboxMessageConsumer);
        }
    }
}