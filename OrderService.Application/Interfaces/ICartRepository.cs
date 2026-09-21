using BuildingBlocks.Interfaces;
using OrderService.Domain.Entities.Carts;

namespace OrderService.Application.Interfaces
{
    public interface ICartRepository : IGenericRepositoryAsync<Cart>
    {
        public Task<Cart?> GetByCustomerIdAsync(Guid customerId,
         CancellationToken cancellationToken);
    }
}
