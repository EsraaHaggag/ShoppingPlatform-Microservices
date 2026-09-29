using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities.Carts;
using PaymentService.Domain.Entities.Payment;

namespace PaymentService.Infrastructure.Persistence
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(
            DbContextOptions<PaymentDbContext> options)
            : base(options)
        {
        }

        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(PaymentDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
