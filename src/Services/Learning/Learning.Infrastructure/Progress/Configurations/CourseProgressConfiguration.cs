using Learning.Domain.Enrollments.Entities;
using Learning.Domain.Progress.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Progress.Configurations
{
    internal sealed class CourseProgressConfiguration : IEntityTypeConfiguration<CourseProgress>
    {
        public void Configure(EntityTypeBuilder<CourseProgress> builder)
        {
            builder.ToTable("CoursesProgress");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.StudentId)
                .IsRequired();

            builder.Property(c => c.CourseId)
                .IsRequired();

            builder.Property(c => c.StartedAt)
                .IsRequired();

            builder.Property(c => c.CompletedAt)
                .IsRequired(false);

            builder.Property(c => c.OverallCompletionPercentage)
                .IsRequired();

            builder.Property(c => c.TotalMinutesWatched)
                .IsRequired();

            builder.Property(c => c.Status)
                .HasConversion<string>()
                .HasColumnType("VARCHAR(160)");

            builder.HasIndex(c => new { c.StudentId, c.CourseId }).IsUnique();

            builder.Metadata.FindNavigation(nameof(CourseProgress.ModulesProgress))!
                .SetField("_moduleProgresses");

            builder.Metadata.FindNavigation(nameof(CourseProgress.ModulesProgress))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}