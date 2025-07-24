using Learnix.Commons.Infrastructure.Inbox.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learnix.Commons.Infrastructure.Inbox.Configurations
{
    public sealed class InboxMessageConsumerConfiguration : IEntityTypeConfiguration<InboxMessageConsumer>
    {
        public void Configure(EntityTypeBuilder<InboxMessageConsumer> builder)
        {
            builder.ToTable("InboxMessageConsumers");

            builder.HasKey(c => new { c.InboxMessageCorrelationId, c.Name });

            builder.Property(c => c.Name)
                .HasColumnType("VARCHAR(256)")
                .IsRequired();
        }
    }
}