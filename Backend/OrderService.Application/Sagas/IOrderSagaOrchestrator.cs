using BuildingBlocks.Common;
using OrderService.Application.Features.Orders.Commands.Checkout;

namespace OrderService.Application.Sagas
{
    public interface IOrderSagaOrchestrator
    {
        Task<Result<CheckoutSagaResponse>> StartAsync(
            Guid orderId,
            Guid customerId,
            CancellationToken cancellationToken);
    }
}
