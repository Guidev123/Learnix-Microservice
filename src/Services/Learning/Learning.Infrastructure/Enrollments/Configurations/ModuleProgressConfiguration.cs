using Learning.Domain.Enrollments.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Enrollments.Configurations
{
    internal sealed class ModuleProgressConfiguration : IEntityTypeConfiguration<ModuleProgress>
    {
        public void Configure(EntityTypeBuilder<ModuleProgress> builder)
        {
            builder.ToTable("ModuleProgress");
            builder.HasKey(m => m.Id);

            builder
                .HasOne<CourseProgress>()
                .WithMany(c => c.Modules)
                .HasForeignKey(m => m.CourseId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.Property(m => m.Status)
                .HasConversion<string>()
                .HasColumnType("VARCHAR(50)");
        }
    }
}