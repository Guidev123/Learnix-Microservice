using Learnix.Commons.Infrastructure.Outbox.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learnix.Commons.Infrastructure.Outbox.Configurations
{
    public sealed class OutboxMessageConsumerConfiguration : IEntityTypeConfiguration<OutboxMessageConsumer>
    {
        public void Configure(EntityTypeBuilder<OutboxMessageConsumer> builder)
        {
            builder.ToTable("OutboxMessageConsumers");

            builder.HasKey(c => new { c.OutboxMessageCorrelationId, c.Name });

            builder.Property(c => c.Name)
                .HasColumnType("VARCHAR(256)")
                .IsRequired();
        }
    }
}