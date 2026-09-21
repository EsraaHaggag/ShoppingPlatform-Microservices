using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities.Carts;
using OrderService.Domain.Entities.Orders;

namespace OrderService.Infrastructure.Persistence
{
    public class OrderDbContext : DbContext
    {

        public OrderDbContext(
            DbContextOptions<OrderDbContext> options)
            : base(options)
        {

        }
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Cart> Carts => Set<Cart>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(OrderDbContext).Assembly);
        }
    }
}
