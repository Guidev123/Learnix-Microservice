using Learning.Domain.Enrollments.Entities;
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

            builder
                .HasOne<Student>()
                .WithMany(e => e.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder
                .HasOne<Course>()
                .WithMany()
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.ClientNoAction);

            builder.Property(c => c.Status)
                .HasConversion<string>()
                .HasColumnType("VARCHAR(50)");
        }
    }
}