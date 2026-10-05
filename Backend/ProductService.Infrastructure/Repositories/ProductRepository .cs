using BuildingBlocks.Repositories;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities.Products;
using ProductService.Infrastructure.Persistence;

namespace ProductService.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepositoryAsync<Product>, IProductRepository
    {
        private readonly DbSet<Product> _products;
        public ProductRepository(ProductDbContext dbContext) : base(dbContext)
        {
            _products = dbContext.Products;
        }

        public async Task<IReadOnlyList<Product>> GetByIdsAsync(
          IReadOnlyCollection<Guid> ids,
          CancellationToken cancellationToken)
        {
            return await _products
                .Include(p => p.Images)
                .Where(p => ids.Contains(p.Id))
                .ToListAsync(cancellationToken);
        }
        public override async Task<Product?> GetByIdAsync(Guid id,
          CancellationToken cancellationToken)
        {
            return await _products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(
                    p => p.Id == id,
                    cancellationToken);
        }
        public IQueryable<Product> GetQueryable()
        {
            return _products
                .Include(p => p.Images)
                .Where(v => v.IsDeleted == false).AsQueryable();
        }
    }
}

