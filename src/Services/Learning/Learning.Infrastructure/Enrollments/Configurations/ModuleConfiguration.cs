using Learning.Domain.Enrollments.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Enrollments.Configurations
{
    internal sealed class ModuleConfiguration : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.ToTable("Modules");
            builder.HasKey(m => m.Id);

            builder
                .HasOne<Course>()
                .WithMany(c => c.Modules)
                .HasForeignKey(m => m.CourseId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.Property(m => m.Status)
                .HasConversion<string>()
                .HasColumnType("VARCHAR(50)");
        }
    }
}