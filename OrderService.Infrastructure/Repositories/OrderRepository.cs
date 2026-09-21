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
    }
}
