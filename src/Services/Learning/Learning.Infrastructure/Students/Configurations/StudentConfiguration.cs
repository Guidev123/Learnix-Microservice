using Learning.Domain.Students.Students.Entities;
using Learnix.Commons.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Students.Configurations
{
    internal sealed class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");

            builder.HasKey(s => s.Id);

            builder.OwnsOne(s => s.Name, name =>
            {
                name.Property(n => n.FirstName)
                    .HasColumnName("FirstName")
                    .HasColumnType("VARCHAR")
                    .IsRequired()
                    .HasMaxLength(Name.NameMaxLength);

                name.Property(n => n.LastName)
                    .HasColumnName("LastName")
                    .HasColumnType("VARCHAR")
                    .IsRequired()
                    .HasMaxLength(Name.NameMaxLength);
            });

            builder.OwnsOne(s => s.Email, email =>
            {
                email.Property(e => e.Address)
                    .HasColumnName(nameof(Email))
                    .HasColumnType("VARCHAR")
                    .IsRequired()
                    .HasMaxLength(Email.MaxEmailLength);

                email.HasIndex(e => e.Address).IsUnique();
            });
        }
    }
}