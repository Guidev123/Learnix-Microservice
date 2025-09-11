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

            builder.Property(c => c.EnrollmentId)
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

            builder.HasOne<Enrollment>()
                .WithOne()
                .HasForeignKey<CourseProgress>(c => c.EnrollmentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(c => new { c.StudentId, c.EnrollmentId, c.CourseId }).IsUnique();
        }
    }
}