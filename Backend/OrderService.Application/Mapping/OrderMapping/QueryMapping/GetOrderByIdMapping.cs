using OrderService.Application.Features.DTOs;
using OrderService.Domain.Entities.Orders;

namespace OrderService.Application.Mapping.OrderMapping
{
    public partial class OrderProfile
    {
        public void GetOrderByIdMapping()
        {
            CreateMap<Order, OrderDto>();
            CreateMap<OrderItem, OrderItemDto>();
        }
    }
}
