using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Users.Models;

namespace Users.Infrastructure.Users.Configurations
{
    internal sealed class PermissionMapping : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions");

            builder.HasKey(p => p.Code);

            builder.Property(p => p.Code).HasColumnType("VARCHAR(100)").IsRequired();

            builder
                .HasMany<Role>()
                .WithMany()
                .UsingEntity(joinBuilder =>
                {
                    joinBuilder.ToTable("RolePermissions");
                });
        }
    }
}