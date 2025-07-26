using Learnix.Commons.Domain.DomainEvents;
using Learnix.Commons.Domain.DomainObjects;
using Learnix.Commons.Infrastructure.Extensions;
using Learnix.Commons.Infrastructure.Outbox.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace Learnix.Commons.Infrastructure.Outbox.Interceptors
{
    public sealed class InsertOutboxMessagesInterceptor : SaveChangesInterceptor
    {
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if (eventData.Context is not null)
                InsertOutboxMessages(eventData.Context);

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private static void InsertOutboxMessages(DbContext context)
        {
            var outboxMessages = context
                     .ChangeTracker
                     .Entries<Entity>()
                     .Select(entry => entry.Entity)
                     .SelectMany(entity =>
                     {
                         IReadOnlyCollection<IDomainEvent> domainEvents = entity.DomainEvents.ToList();

                         entity.ClearDomainEvents();

                         return domainEvents;
                     })
                     .Select(domainEvent => new OutboxMessage(
                         domainEvent.CorrelationId,
                         domainEvent.Messagetype,
                         JsonConvert.SerializeObject(domainEvent, SerializerExtensions.Instance),
                         domainEvent.OccurredOn)
                     ).ToList();

            context.Set<OutboxMessage>().AddRange(outboxMessages);
        }
    }
}