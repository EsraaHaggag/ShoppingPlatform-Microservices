using BuildingBlocks.Repositories;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities.Carts;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Infrastructure.Repositories
{
    public class CartRepository : GenericRepositoryAsync<Cart>, ICartRepository
    {
        private readonly DbSet<Cart> _carts;
        public CartRepository(OrderDbContext dbContext) : base(dbContext)
        {
            _carts = dbContext.Carts;
        }

        public async Task<Cart?> GetByCustomerIdAsync(Guid customerId,
         CancellationToken cancellationToken)
        {
            return await _carts
                .Include(x => x.Items)
                .FirstOrDefaultAsync(
                    c => c.CustomerId == customerId,
                    cancellationToken);
        }
    }
}
