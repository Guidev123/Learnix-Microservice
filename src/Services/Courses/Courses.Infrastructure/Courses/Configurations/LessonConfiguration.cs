using Courses.Domain.Courses.Entities;
using Courses.Domain.Courses.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Courses.Infrastructure.Courses.Configurations
{
    internal sealed class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.ToTable("Lessons");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.Title)
                .HasColumnType("VARCHAR")
                .HasMaxLength(Lesson.MaxTitleLength)
                .IsRequired();

            builder.Property(m => m.OrderIndex).IsRequired();

            builder.OwnsOne(l => l.Video, video =>
            {
                video.Property(v => v.Url)
                    .HasColumnName("VideoUrl")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(Video.MaxUrlLength)
                    .IsRequired();

                video.Property(v => v.ThumbnailUrl)
                    .HasColumnName("VideoThumbnailUrl")
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(Video.MaxThumbnailUrlLength)
                    .IsRequired();
            });

            builder.HasOne<Module>()
                .WithMany(m => m.Lessons)
                .HasForeignKey(l => l.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(l => new { l.ModuleId, l.OrderIndex }).IsUnique();
        }
    }
}