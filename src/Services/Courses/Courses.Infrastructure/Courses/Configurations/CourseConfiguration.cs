using Courses.Domain.Courses.Entities;
using Courses.Domain.Courses.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Courses.Infrastructure.Courses.Configurations
{
    internal sealed class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses");

            builder.HasKey(c => c.Id);

            builder.OwnsOne(c => c.Specification, specification =>
            {
                specification.Property(sp => sp.Title)
                    .HasColumnName("Title")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(CourseSpecification.MaxTitleLength)
                    .IsRequired();

                specification.Property(sp => sp.Description)
                    .HasColumnName("Description")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(CourseSpecification.MaxDescriptionLength)
                    .IsRequired();
            });

            builder.Ignore(c => c.ModulesQuantity);
            builder.Ignore(c => c.DurationInHours);

            builder.Property(c => c.DificultLevel)
                .HasColumnType("VARCHAR(50)")
                .HasConversion<string>()
                .IsRequired();

            builder.HasOne<Category>()
                .WithMany()
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}