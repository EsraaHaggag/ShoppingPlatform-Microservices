using BuildingBlocks.Repositories;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities.Orders;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Infrastructure.Repositories
{
    public class OrderRepository : GenericRepositoryAsync<Order>, IOrderRepository
    {
        private readonly DbSet<Order> _orders;
        public OrderRepository(OrderDbContext dbContext) : base(dbContext)
        {
            _orders = dbContext.Set<Order>();
        }

        public async Task<Order?> GetByIdAsync(Guid orderId, Guid customerId,
          CancellationToken cancellationToken)
        {
            return await _orders
                .FirstOrDefaultAsync(
                    x => x.Id == orderId &&
                         x.CustomerId == customerId,
                    cancellationToken);
        }

        public async Task<IReadOnlyList<Order>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken)
        {
            return await _orders
                .Include(x => x.Items)
                .Where(x => x.CustomerId == customerId)
                .ToListAsync(cancellationToken);
        }
    }
}
