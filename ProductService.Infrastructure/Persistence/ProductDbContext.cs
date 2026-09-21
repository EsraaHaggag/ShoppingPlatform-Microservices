using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities.Products;

namespace ProductService.Infrastructure.Persistence
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(
            DbContextOptions<ProductDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products => Set<Product>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ProductDbContext).Assembly);
        }
    }
}
