using Learnix.Commons.Infrastructure.Inbox.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learnix.Commons.Infrastructure.Inbox.Configurations
{
    public sealed class InboxMessageConfiguration : IEntityTypeConfiguration<InboxMessage>
    {
        public void Configure(EntityTypeBuilder<InboxMessage> builder)
        {
            builder.ToTable("InboxMessages");

            builder.HasKey(im => im.CorrelationId);

            builder.Property(x => x.Type).HasColumnType("VARCHAR(200)").IsRequired();
            builder.Property(x => x.Content).HasColumnType("VARCHAR(MAX)").IsRequired();
            builder.Property(x => x.OccurredOn).IsRequired();
            builder.Property(x => x.Error).HasColumnType("VARCHAR(256)").IsRequired(false);
            builder.Property(x => x.ProcessedOn).IsRequired(false);
        }
    }
}