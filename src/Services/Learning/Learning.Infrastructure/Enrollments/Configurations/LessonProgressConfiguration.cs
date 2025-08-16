using Learning.Domain.Enrollments.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Enrollments.Configurations
{
    internal sealed class LessonProgressConfiguration : IEntityTypeConfiguration<LessonProgress>
    {
        public void Configure(EntityTypeBuilder<LessonProgress> builder)
        {
            builder.ToTable("LessonProgress");
            builder.HasKey(l => l.Id);

            builder
                .HasOne<ModuleProgress>()
                .WithMany(m => m.Lessons)
                .HasForeignKey(l => l.ModuleId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}