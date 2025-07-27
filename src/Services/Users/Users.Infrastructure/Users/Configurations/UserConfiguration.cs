using Learnix.Commons.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities;

namespace Users.Infrastructure.Users.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.OwnsOne(u => u.Name, name =>
            {
                name.Property(u => u.FirstName)
                .HasColumnName(nameof(Name.FirstName))
                .HasColumnType("VARCHAR(50)");

                name.Property(u => u.LastName)
                .HasColumnName(nameof(Name.LastName))
                .HasColumnType("VARCHAR(50)");
            });

            builder.OwnsOne(u => u.Email, email =>
            {
                email.Property(u => u.Address)
                .HasColumnName(nameof(Email))
                .HasColumnType("VARCHAR(160)");

                email.HasIndex(u => u.Address).IsUnique();
            });

            builder.OwnsOne(u => u.Age, age =>
            {
                age.Property(u => u.BirthDate)
                .HasColumnName(nameof(Age.BirthDate));
            });

            builder.HasIndex(u => u.IdentityId).IsUnique();
        }
    }
}