using Learning.Domain.Enrollments.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Enrollments.Configurations
{
    internal sealed class CourseProgressConfiguration : IEntityTypeConfiguration<CourseProgress>
    {
        public void Configure(EntityTypeBuilder<CourseProgress> builder)
        {
            builder.ToTable("CourseProgress");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Status)
                .HasConversion<string>()
                .HasColumnType("VARCHAR(50)");

            builder.OwnsOne(c => c.ProgressDateRange, progress =>
            {
                progress.Property(p => p.StartedAt)
                    .HasColumnName("StartedAt")
                    .IsRequired();

                progress.Property(p => p.CompletedAt)
                    .HasColumnName("CompletedAt")
                    .IsRequired(false);
            });
        }
    }
}