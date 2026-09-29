using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Entities.Carts;

namespace PaymentService.Infrastructure.Configurations
{
    public class OutboxMessageConfiguration
    : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(
            EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Topic)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Key)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Payload)
                .IsRequired();

            builder.HasIndex(x => x.ProcessedOnUtc);
        }
    }
}
