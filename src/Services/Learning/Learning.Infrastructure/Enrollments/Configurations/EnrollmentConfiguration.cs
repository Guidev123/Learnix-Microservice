using Learning.Domain.Enrollments.Entities;
using Learning.Domain.Progress.Entities;
using Learning.Domain.Students.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Enrollments.Configurations
{
    internal sealed class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.ToTable("Enrollments");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.CourseId)
                .IsRequired();

            builder
                .HasOne<Student>()
                .WithMany(e => e.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder
                .HasOne<CourseProgress>()
                .WithMany()
                .HasForeignKey(e => e.CourseProgressId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.Property(c => c.Status)
                .HasConversion<string>()
                .HasColumnType("VARCHAR(50)");
        }
    }
}