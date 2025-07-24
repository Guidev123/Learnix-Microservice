﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities;
using Users.Domain.Models;

namespace Users.Infrastructure.Persistence.Configurations
{
    internal sealed class RoleMapping : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");

            builder.HasKey(r => r.Name);

            builder.Property(r => r.Name).HasColumnType("VARCHAR(50)").IsRequired();

            builder
                .HasMany<User>()
                .WithMany(u => u.Roles)
                .UsingEntity(joinBuilder =>
                {
                    joinBuilder.ToTable("UserRoles");

                    joinBuilder.Property("RolesName").HasColumnName("RoleName");
                });
        }
    }
}