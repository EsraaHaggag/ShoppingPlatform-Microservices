using BuildingBlocks.Interfaces;
using OrderService.Domain.Entities.Orders;

namespace OrderService.Application.Interfaces
{
    public interface IOrderRepository : IGenericRepositoryAsync<Order>
    {
        public Task<Order?> GetByIdAsync(Guid orderId, Guid customerId,
          CancellationToken cancellationToken);

        public Task<IReadOnlyList<Order>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken);
    }
}
