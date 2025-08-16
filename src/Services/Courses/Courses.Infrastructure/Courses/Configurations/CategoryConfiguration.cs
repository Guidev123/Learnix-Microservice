using Courses.Domain.Courses.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Courses.Infrastructure.Courses.Configurations
{
    internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .HasColumnName("Name")
                .HasColumnType("VARCHAR")
                .HasMaxLength(Category.MaxNameLength)
                .IsRequired();
        }
    }
}