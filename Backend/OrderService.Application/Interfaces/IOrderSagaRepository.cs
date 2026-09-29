using BuildingBlocks.Interfaces;
using OrderService.Domain.Entities.Orders;

namespace OrderService.Application.Interfaces
{
    public interface IOrderSagaRepository : IGenericRepositoryAsync<OrderSaga>
    {
        public Task<OrderSaga?> GetByOrderIdAsync(
            Guid orderId,
            CancellationToken cancellationToken);
    }
}
