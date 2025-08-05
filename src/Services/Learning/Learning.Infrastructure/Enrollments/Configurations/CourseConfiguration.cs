using Learning.Domain.Enrollments.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Enrollments.Configurations
{
    internal sealed class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses");
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