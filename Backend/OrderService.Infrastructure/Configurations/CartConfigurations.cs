using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Entities.Carts;

namespace OrderService.Infrastructure.Configurations
{
    public class CartConfiguration :
    IEntityTypeConfiguration<Cart>
    {
        public void Configure(
            EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CustomerId)
                .IsRequired();
            builder.HasIndex(x => x.CustomerId)
                .IsUnique();
            builder.HasMany(x => x.Items)
            .WithOne().HasForeignKey("CartId")
              .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(x => x.Items)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

        }
    }
}
