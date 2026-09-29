using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Entities.Orders;

namespace OrderService.Infrastructure.Configurations
{
    public class OrderSagaConfiguration
    : IEntityTypeConfiguration<OrderSaga>
    {
        public void Configure(EntityTypeBuilder<OrderSaga> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.OrderId)
                .IsUnique();

            builder.Property(x => x.Status)
                .HasConversion<string>();

            builder.Property(x => x.FailureReason)
                .HasMaxLength(500);
        }
    }
}
