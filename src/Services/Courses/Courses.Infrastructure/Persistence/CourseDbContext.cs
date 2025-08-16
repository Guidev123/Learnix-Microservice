using Courses.Domain.Courses.Entities;
using Learnix.Commons.Domain.Abstractions;
using Learnix.Commons.Infrastructure.Inbox.Configurations;
using Learnix.Commons.Infrastructure.Outbox.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Module = Courses.Domain.Courses.Entities.Module;

namespace Courses.Infrastructure.Persistence
{
    internal sealed class CourseDbContext(DbContextOptions<CourseDbContext> options) : DbContext(options), IUnitOfWork
    {
        internal DbSet<Category> Categories { get; set; } = default!;
        internal DbSet<Course> Courses { get; set; } = default!;
        internal DbSet<Module> Modules { get; set; } = default!;
        internal DbSet<Lesson> Lessons { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schemas.Courses);

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