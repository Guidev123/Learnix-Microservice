using Learning.Domain.Progress.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Progress.Configurations
{
    internal sealed class ModuleProgressConfiguration : IEntityTypeConfiguration<ModuleProgress>
    {
        public void Configure(EntityTypeBuilder<ModuleProgress> builder)
        {
            builder.ToTable("ModulesProgress");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.ModuleId)
                .IsRequired();

            builder.Property(m => m.CompletionPercentage)
                .IsRequired();

            builder.Property(m => m.Status)
                .HasConversion<string>()
                .HasColumnType("VARCHAR(160)");

            builder.Property(m => m.StartedAt)
                .IsRequired(false);

            builder.Property(m => m.CompletedAt)
                .IsRequired(false);

            builder.HasOne<CourseProgress>()
                .WithMany(cp => cp.ModulesProgress)
                .HasForeignKey(m => m.CourseProgressId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(m => new { m.CourseProgressId, m.ModuleId }).IsUnique();
        }
    }
}