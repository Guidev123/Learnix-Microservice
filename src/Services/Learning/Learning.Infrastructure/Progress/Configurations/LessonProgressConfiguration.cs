using Learning.Domain.Progress.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Progress.Configurations
{
    internal sealed class LessonProgressConfiguration : IEntityTypeConfiguration<LessonProgress>
    {
        public void Configure(EntityTypeBuilder<LessonProgress> builder)
        {
            builder.ToTable("LessonsProgress");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.LessonId)
                .IsRequired();

            builder.Property(l => l.ModuleId)
                .IsRequired();

            builder.Property(l => l.CompletionPercentage)
                .IsRequired();

            builder.Property(l => l.Status)
                .HasConversion<string>()
                .HasColumnType("VARCHAR(160)");

            builder.Property(l => l.StartedAt)
                .IsRequired(false);

            builder.Property(l => l.CompletedAt)
                .IsRequired(false);

            builder.Property(l => l.MinutesWatched)
                .IsRequired();

            builder.HasOne<ModuleProgress>()
                .WithMany(cp => cp.LessonsProgress)
                .HasForeignKey(l => l.ModuleProgressId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(l => new { l.ModuleProgressId, l.LessonId }).IsUnique();
        }
    }
}