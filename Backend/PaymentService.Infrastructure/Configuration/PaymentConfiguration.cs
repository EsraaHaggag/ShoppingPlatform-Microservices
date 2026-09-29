using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentService.Domain.Entities.Payment;

namespace PaymentService.Infrastructure.Configuration
{
    public class PaymentConfiguration
    : IEntityTypeConfiguration<Payment>
    {
        public void Configure(
            EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.OrderId)
                .IsRequired();

            builder.Property(x => x.CustomerId)
                .IsRequired();

            builder.Property(x => x.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired();

            builder.Property(x => x.ProviderPaymentId)
                .HasMaxLength(200);

            builder.Property(x => x.CreatedAt)
                .IsRequired();


        }
    }
}
