using BuildingBlocks.Repositories;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities.Orders;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Infrastructure.Repositories
{
    //public class OrderSagaRepository : GenericRepositoryAsync<OrderSaga>, IOrderSagaRepository
    //{
    //    private readonly DbSet<OrderSaga> _orderSagas;
    //    public OrderSagaRepository(OrderDbContext dbContext) : base(dbContext)
    //    {
    //        _orderSagas = dbContext.OrderSagas;
    //    }

    //    public async Task<OrderSaga?> GetByOrderIdAsync(
    //        Guid orderId,
    //        CancellationToken cancellationToken)
    //    {
    //        return await _orderSagas
    //            .FirstOrDefaultAsync(
    //                saga => saga.OrderId == orderId,
    //                cancellationToken);
    //    }


    public class OrderSagaRepository
        : GenericRepositoryAsync<OrderSaga>, IOrderSagaRepository
    {
        private readonly OrderDbContext _dbContext;
        private readonly DbSet<OrderSaga> _orderSagas;

        public OrderSagaRepository(OrderDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
            _orderSagas = dbContext.OrderSagas;
        }

        public async Task<OrderSaga?> GetByOrderIdAsync(
            Guid orderId,
            CancellationToken cancellationToken)
        {
            Console.WriteLine(
                $"🔎 Searching Saga for OrderId: {orderId}");

            Console.WriteLine(
                $"🗄️ Database: {_dbContext.Database.GetDbConnection().Database}");

            Console.WriteLine(
                $"🖥️ Server: {_dbContext.Database.GetDbConnection().DataSource}");

            var saga = await _orderSagas
                .FirstOrDefaultAsync(
                    saga => saga.OrderId == orderId,
                    cancellationToken);

            Console.WriteLine(
                saga is null
                    ? "❌ Saga NOT FOUND"
                    : $"✅ Saga FOUND - SagaId: {saga.Id}");

            return saga;
        }



    }
}
