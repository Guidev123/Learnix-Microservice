using Learnix.Commons.Domain.Abstractions;
using Learnix.Commons.Infrastructure.Inbox.Configurations;
using Learnix.Commons.Infrastructure.Outbox.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Learning.Infrastructure.Persistence
{
    internal sealed class LearningDbContext : DbContext, IUnitOfWork
    {
        public LearningDbContext(DbContextOptions<LearningDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schemas.Learning);

            modelBuilder.ApplyConfiguration(new InboxMessageConfiguration());
            modelBuilder.ApplyConfiguration(new InboxMessageConsumerConfiguration());
            modelBuilder.ApplyConfiguration(new OutboxMessageConsumerConfiguration());
            modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
            => await SaveChangesAsync(cancellationToken) > 0;
    }
}