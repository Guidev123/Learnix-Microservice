using Courses.Domain.Courses.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Courses.Infrastructure.Courses.Configurations
{
    internal sealed class ModuleConfiguration : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.ToTable("Modules");

            builder.HasKey(m => m.Id);

            builder.Ignore(c => c.TotalLessons);
            builder.Ignore(c => c.DurationInHours);

            builder.Property(m => m.OrderIndex).IsRequired();

            builder.Property(m => m.Title)
                .HasMaxLength(Module.MaxTitleLength)
                .HasColumnType("VARCHAR")
                .IsRequired();

            builder.HasOne<Course>()
                .WithMany(c => c.Modules)
                .HasForeignKey(m => m.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(m => new { m.CourseId, m.OrderIndex }).IsUnique();
        }
    }
}