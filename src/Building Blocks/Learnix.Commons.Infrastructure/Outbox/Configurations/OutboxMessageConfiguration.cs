using Learnix.Commons.Infrastructure.Outbox.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learnix.Commons.Infrastructure.Outbox.Configurations
{
    public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable("OutboxMessages");

            builder.HasKey(x => x.CorrelationId);

            builder.Property(x => x.Type).HasColumnType("VARCHAR(200)");
            builder.Property(x => x.Content).HasColumnType("VARCHAR(3000)");
            builder.Property(x => x.Error).HasColumnType("VARCHAR(256)");
        }
    }
}