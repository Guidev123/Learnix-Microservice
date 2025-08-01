﻿using Dapper;
using Learning.Infrastructure.Persistence;
using Learnix.Commons.Application.Clock;
using Learnix.Commons.Application.Exceptions;
using Learnix.Commons.Application.Factories;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Infrastructure.Extensions;
using Learnix.Commons.Infrastructure.Inbox.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;
using System.Data;

namespace Learning.Infrastructure.Inbox
{
    [DisallowConcurrentExecution]
    internal sealed class ProcessInboxJob(
        ISqlConnectionFactory sqlConnectionFactory,
        ILogger<ProcessInboxJob> logger,
        IServiceScopeFactory serviceScopeFactory,
        IDateTimeProvider dateTimeProvider,
        IOptions<InboxOptions> options) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            logger.LogInformation("{Module} - Beginning to process inbox messages", Schemas.Learning);

            using var connection = sqlConnectionFactory.Create();
            await connection.OpenAsync();

            using var transaction = await connection.BeginTransactionAsync();

            var inboxMessages = await GetInboxMessagesAsync(connection, transaction, options.Value.BatchSize);

            if (inboxMessages.Count == 0)
            {
                await transaction.CommitAsync();
                logger.LogInformation("{Module} - No inbox messages to process", Schemas.Learning);
                return;
            }

            foreach (var inboxMessage in inboxMessages)
            {
                Exception? exception = null;

                try
                {
                    var integrationEvent = JsonConvert.DeserializeObject<IIntegrationEvent>(inboxMessage.Content, SerializerExtensions.Instance)!;

                    using var scope = serviceScopeFactory.CreateScope();

                    var integrationEventHandlers = IntegrationEventHandlersFactory.GetHandlers(
                        integrationEvent.GetType(),
                        scope.ServiceProvider,
                        typeof(LearningModule).Assembly);

                    var handlerTasks = integrationEventHandlers.Select(handler => handler.ExecuteAsync(integrationEvent));
                    await Task.WhenAll(handlerTasks);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "{Module} - Exception while processing inbox message {CorrelationId}", Schemas.Learning, inboxMessage.CorrelationId);

                    exception = ex;
                }

                await UpdateInboxMessageAsync(connection, transaction, inboxMessage, exception);
            }

            await transaction.CommitAsync();

            logger.LogInformation("{Module} - Completed process inbox messages", Schemas.Learning);
        }

        private static async Task<IReadOnlyList<InboxMessageResponse>> GetInboxMessagesAsync(
           IDbConnection connection,
           IDbTransaction transaction,
           int batchSize)
        {
            var sql = @$"
                SELECT TOP ({batchSize})
                    CorrelationId,
                    Content
                FROM learning.InboxMessages WITH (UPDLOCK, ROWLOCK)
                WHERE ProcessedOn IS NULL
                ORDER BY OccurredOn";

            var inboxMessages = await connection.QueryAsync<InboxMessageResponse>(sql, transaction: transaction);

            return inboxMessages.ToList();
        }

        private async Task UpdateInboxMessageAsync(
            IDbConnection connection,
            IDbTransaction transaction,
            InboxMessageResponse inboxMessage,
            Exception? exception)
        {
            var sql = @$"
                UPDATE learning.InboxMessages
                SET ProcessedOn = @ProcessedOn,
                    Error = @Error
                WHERE CorrelationId = @CorrelationId";

            await connection.ExecuteAsync(sql, new
            {
                inboxMessage.CorrelationId,
                ProcessedOnUtc = dateTimeProvider.UtcNow,
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