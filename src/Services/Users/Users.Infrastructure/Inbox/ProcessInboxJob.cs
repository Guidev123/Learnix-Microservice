using Dapper;
using Learnix.Commons.Application.Clock;
using Learnix.Commons.Application.Exceptions;
using Learnix.Commons.Application.Factories;
using Learnix.Commons.Domain.DomainEvents;
using Learnix.Commons.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MidR.MemoryQueue.Interfaces;
using Newtonsoft.Json;
using Quartz;
using System.Data;
using Users.Infrastructure.Persistence;

namespace Users.Infrastructure.Inbox
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
            logger.LogInformation("{Module} - Beginning to process inbox messages", Schemas.Users);

            using var connection = sqlConnectionFactory.Create();
            await connection.OpenAsync();

            using var transaction = await connection.BeginTransactionAsync();

            var inboxMessages = await GetInboxMessagesAsync(connection, transaction, options.Value.BatchSize);

            if (inboxMessages.Count == 0)
            {
                await transaction.CommitAsync();
                logger.LogInformation("{Module} - No inbox messages to process", Schemas.Users);
                return;
            }

            using var scope = serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            foreach (var inboxMessage in inboxMessages)
            {
                Exception? exception = null;

                try
                {
                    await mediator.PublishAsync(JsonConvert.DeserializeObject<IDomainEvent>(inboxMessage.Content, SerializerExtensions.Instance)!);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "{Module} - Exception while processing inbox message {CorrelationId}", Schemas.Users, inboxMessage.CorrelationId);

                    exception = ex;
                }

                await UpdateInboxMessageAsync(connection, transaction, inboxMessage, exception);
            }

            await transaction.CommitAsync();

            logger.LogInformation("{Module} - Completed process inbox messages", Schemas.Users);
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
                FROM users.InboxMessages WITH (UPDLOCK, ROWLOCK)
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
                UPDATE users.InboxMessages
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