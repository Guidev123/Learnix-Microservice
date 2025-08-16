using Courses.Infrastructure.Persistence;
using Dapper;
using Learnix.Commons.Application.Clock;
using Learnix.Commons.Application.Exceptions;
using Learnix.Commons.Application.Factories;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Infrastructure.Extensions;
using Learnix.Commons.Infrastructure.Inbox.Factories;
using Learnix.Commons.Infrastructure.Inbox.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;
using System.Data;

namespace Courses.Infrastructure.Inbox
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
            logger.LogInformation("{ModuleProgress} - Beginning to process inbox messages", Schemas.Courses);

            using var connection = sqlConnectionFactory.Create();
            await connection.OpenAsync();

            using var transaction = await connection.BeginTransactionAsync();

            var inboxMessages = await GetInboxMessagesAsync(connection, transaction, options.Value.BatchSize);

            if (inboxMessages.Count == 0)
            {
                await transaction.CommitAsync();
                logger.LogInformation("{ModuleProgress} - No inbox messages to process", Schemas.Courses);
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
                        typeof(CourseModule).Assembly);

                    var handlerTasks = integrationEventHandlers.Select(handler => handler.ExecuteAsync(integrationEvent));
                    await Task.WhenAll(handlerTasks);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "{ModuleProgress} - Exception while processing inbox message {CorrelationId}", Schemas.Courses, inboxMessage.CorrelationId);

                    exception = ex;
                }

                await UpdateInboxMessageAsync(connection, transaction, inboxMessage, exception);
            }

            await transaction.CommitAsync();

            logger.LogInformation("{ModuleProgress} - Completed process inbox messages", Schemas.Courses);
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
                FROM courses.InboxMessages WITH (UPDLOCK, ROWLOCK)
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
                UPDATE courses.InboxMessages
                SET ProcessedOn = @ProcessedOn,
                    Error = @Error
                WHERE CorrelationId = @CorrelationId";

            await connection.ExecuteAsync(sql, new
            {
                inboxMessage.CorrelationId,
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