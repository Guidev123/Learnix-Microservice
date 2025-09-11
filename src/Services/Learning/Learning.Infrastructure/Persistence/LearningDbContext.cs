using Learning.Domain.Enrollments.Entities;
using Learning.Domain.Progress.Entities;
using Learning.Domain.Students.Entities;
using Learnix.Commons.Domain.Abstractions;
using Learnix.Commons.Infrastructure.Inbox.Configurations;
using Learnix.Commons.Infrastructure.Outbox.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ModuleProgress = Learning.Domain.Progress.Entities.ModuleProgress;

namespace Learning.Infrastructure.Persistence
{
    internal sealed class LearningDbContext : DbContext, IUnitOfWork
    {
        public LearningDbContext(DbContextOptions<LearningDbContext> options) : base(options)
        { }

        internal DbSet<Student> Students { get; set; } = default!;
        internal DbSet<Enrollment> Enrollments { get; set; } = default!;
        internal DbSet<CourseProgress> Courses { get; set; } = default!;
        internal DbSet<ModuleProgress> Modules { get; set; } = default!;
        internal DbSet<LessonProgress> Lessons { get; set; } = default!;
        internal DbSet<CourseProgress> CoursesProgress { get; set; } = default!;
        internal DbSet<ModuleProgress> ModulesProgress { get; set; } = default!;
        internal DbSet<LessonProgress> LessonsProgress { get; set; } = default!;

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