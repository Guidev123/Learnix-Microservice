using Dapper;
using Learning.Infrastructure.Persistence;
using Learnix.Commons.Application.Clock;
using Learnix.Commons.Application.Exceptions;
using Learnix.Commons.Application.Factories;
using Learnix.Commons.Domain.DomainEvents;
using Learnix.Commons.Infrastructure.Extensions;
using Learnix.Commons.Infrastructure.Outbox.Factories;
using Learnix.Commons.Infrastructure.Outbox.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;
using System.Data;

namespace Learning.Infrastructure.Outbox
{
    [DisallowConcurrentExecution]
    internal sealed class ProcessOutboxJob(
        ISqlConnectionFactory sqlConnectionFactory,
        ILogger<ProcessOutboxJob> logger,
        IServiceScopeFactory serviceScopeFactory,
        IDateTimeProvider dateTimeProvider,
        IOptions<OutboxOptions> options) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            logger.LogInformation("{ModuleProgress} - Beginning to process outbox messages", Schemas.Learning);

            using var connection = sqlConnectionFactory.Create();
            await connection.OpenAsync();

            using var transaction = await connection.BeginTransactionAsync();

            var outboxMessages = await GetOutboxMessagesAsync(connection, transaction, options.Value.BatchSize);

            if (outboxMessages.Count == 0)
            {
                await transaction.CommitAsync();
                logger.LogInformation("{ModuleProgress} - No outbox messages to process", Schemas.Learning);
                return;
            }

            foreach (var outboxMessage in outboxMessages)
            {
                Exception? exception = null;

                try
                {
                    var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Content, SerializerExtensions.Instance)!;

                    using var scope = serviceScopeFactory.CreateScope();

                    var domainEventHandlers = DomainEventHandlersFactory.GetHandlers(
                                            domainEvent.GetType(),
                                            scope.ServiceProvider,
                                            Application.AssemblyReference.Assembly);

                    var handlerTasks = domainEventHandlers.Select(handler => handler.ExecuteAsync(domainEvent));
                    await Task.WhenAll(handlerTasks);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "{ModuleProgress} - Exception while processing outbox message {CorrelationId}", Schemas.Learning, outboxMessage.CorrelationId);

                    exception = ex;
                }

                await UpdateOutboxMessageAsync(connection, transaction, outboxMessage, exception);
            }

            await transaction.CommitAsync();

            logger.LogInformation("{ModuleProgress} - Completed process outbox messages", Schemas.Learning);
        }

        private static async Task<IReadOnlyList<OutboxMessageResponse>> GetOutboxMessagesAsync(
           IDbConnection connection,
           IDbTransaction transaction,
           int batchSize)
        {
            var sql = @$"
                SELECT TOP ({batchSize})
                    CorrelationId,
                    Content
                FROM learning.OutboxMessages WITH (UPDLOCK, ROWLOCK)
                WHERE ProcessedOn IS NULL
                ORDER BY OccurredOn";

            var outboxMessages = await connection.QueryAsync<OutboxMessageResponse>(sql, transaction: transaction);

            return outboxMessages.ToList();
        }

        private async Task UpdateOutboxMessageAsync(
            IDbConnection connection,
            IDbTransaction transaction,
            OutboxMessageResponse outboxMessage,
            Exception? exception)
        {
            var sql = @$"
                UPDATE learning.OutboxMessages
                SET ProcessedOn = @ProcessedOn,
                    Error = @Error
                WHERE CorrelationId = @CorrelationId";

            await connection.ExecuteAsync(sql, new
            {
                outboxMessage.CorrelationId,
                ProcessedOn = dateTimeProvider.UtcNow,
                Error = GetExceptionMessage(exception)
            }, transaction: transaction);
        }

        private static string? GetExceptionMessage(Exception? exception)
        {
            if (exception is null)
                return null;

            return exception switch
            {
                LearnixException learnixException when learnixException.Error?.Description is not null => learnixException.Error.Description,
                _ when exception.InnerException?.Message is not null => exception.InnerException.Message,
                _ => exception.Message
            };
        }
    }
}