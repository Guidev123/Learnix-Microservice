using Dapper;
using Learnix.Commons.Application.Factories;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Domain.DomainEvents;
using Learnix.Commons.Infrastructure.Outbox.Models;
using System.Data.Common;

namespace Learning.Infrastructure.Outbox
{
    internal sealed class IdempotentDomainEventHandlerDecorator<TDomainEvent>(
        DomainEventHandler<TDomainEvent> innerHandler,
        ISqlConnectionFactory sqlConnectionFactory) : DomainEventHandler<TDomainEvent>
        where TDomainEvent : DomainEvent
    {
        public override async Task ExecuteAsync(TDomainEvent notification, CancellationToken cancellationToken)
        {
            using var connection = sqlConnectionFactory.Create();

            var outboxMessageConsumer = new OutboxMessageConsumer(notification.CorrelationId, notification.Messagetype);

            if (await IsOutboxMessageProcessedAsync(outboxMessageConsumer, connection))
            {
                return;
            }

            await innerHandler.ExecuteAsync(notification, cancellationToken);

            await MarkOutboxMessageAsProcessedAsync(outboxMessageConsumer, connection);
        }

        private static async Task<bool> IsOutboxMessageProcessedAsync(
          OutboxMessageConsumer outboxMessageConsumer,
          DbConnection connection)
        {
            var sql = $@"
                SELECT CASE
                    WHEN EXISTS(
                        SELECT 1
                        FROM learning.OutboxMessageConsumers
                        WHERE OutboxMessageCorrelationId = @OutboxMessageCorrelationId
                        AND Name = @Name)
                   THEN 1
                   ELSE 0
                END";

            return await connection.ExecuteScalarAsync<bool>(sql, outboxMessageConsumer);
        }

        private static async Task MarkOutboxMessageAsProcessedAsync(
            OutboxMessageConsumer outboxMessageConsumer,
            DbConnection connection)
        {
            var sql = $@"
                INSERT INTO learning.OutboxMessageConsumers (OutboxMessageCorrelationId, Name)
                VALUES (@OutboxMessageCorrelationId, @Name)";

            await connection.ExecuteAsync(sql, outboxMessageConsumer);
        }
    }
}