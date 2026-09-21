using BuildingBlocks.Interfaces;
using OrderService.Domain.Entities.Orders;

namespace OrderService.Application.Interfaces
{
    public interface IOrderRepository : IGenericRepositoryAsync<Order>
    {

    }
}
