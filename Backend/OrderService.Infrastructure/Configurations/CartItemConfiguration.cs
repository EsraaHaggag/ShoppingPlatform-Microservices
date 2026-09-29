using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Entities.Carts;

namespace OrderService.Infrastructure.Configurations
{
    public class CartItemConfiguration :
    IEntityTypeConfiguration<CartItem>
    {
        public void Configure(
            EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ProductId)
                .IsRequired();
            builder.Property(x => x.ProductName)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(x => x.UnitPrice)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.Quantity)
                .IsRequired();
        }
    }
}
