using Learnix.Commons.Domain.Abstractions;
using Learnix.Commons.Infrastructure.Inbox.Configurations;
using Learnix.Commons.Infrastructure.Outbox.Configurations;
using Learnix.Commons.Infrastructure.Outbox.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Users.Domain.Entities;
using Users.Domain.Models;

namespace Users.Infrastructure.Persistence
{
    internal sealed class UsersDbContext : DbContext, IUnitOfWork
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> contextOptions)
            : base(contextOptions)
        { }

        internal DbSet<User> Users { get; set; } = default!;
        internal DbSet<Role> Roles { get; set; } = default!;
        internal DbSet<Permission> Permissions { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schemas.Users);

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