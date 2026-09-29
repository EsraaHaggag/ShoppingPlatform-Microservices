using BuildingBlocks.Common;
using BuildingBlocks.Interfaces;
using BuildingBlocks.Stock;
using OrderService.Application.Features.Orders.Commands.Checkout;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities.Orders;
using OrderService.Domain.Entities.Orders.Errors;
using OrderService.Domain.Enums;

namespace OrderService.Application.Sagas
{
    public class OrderSagaOrchestrator : IOrderSagaOrchestrator
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderSagaRepository _orderSagaRepository;
        private readonly IOutboxRepository _outboxRepository;
        private readonly IProductServiceClient _productServiceClient;
        private readonly IPaymentServiceClient _paymentServiceClient;

        public OrderSagaOrchestrator(
            IOrderRepository orderRepository,
            IOrderSagaRepository orderSagaRepository,
            IOutboxRepository outboxRepository,
            IProductServiceClient productServiceClient,
            IPaymentServiceClient paymentServiceClient)
        {
            _orderRepository = orderRepository;
            _orderSagaRepository = orderSagaRepository;
            _outboxRepository = outboxRepository;
            _productServiceClient = productServiceClient;
            _paymentServiceClient = paymentServiceClient;
        }

        public async Task<Result<CheckoutSagaResponse>> StartAsync(
          Guid orderId, Guid customerId,
          CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(
                orderId, customerId,
                cancellationToken);

            if (order is null)
            {
                return Result<CheckoutSagaResponse>.Failure(
                    OrderErrors.OrderNotFound);
            }

            var saga = new OrderSaga(
                order.Id,
                customerId);

            await _orderSagaRepository.AddAsync(
                saga);

            saga.SetStatus(OrderSagaStatus.StockReservationPending);
            await _orderSagaRepository.CompleteAsync(
                cancellationToken);


            // Reserve Stock
            var reserveItems = order.Items
                .Select(item => new ReserveStockItem(
                    item.ProductId,
                    item.Quantity,
                    item.UnitPrice))
                .ToList();

            var reserveResult =
                await _productServiceClient.ReserveStockAsync(
                    reserveItems,
                    cancellationToken);

            if (reserveResult.IsFailure)
            {
                saga.Fail(
                    reserveResult.Error.Message);

                await _orderSagaRepository.CompleteAsync(
                    cancellationToken);

                return Result<CheckoutSagaResponse>.Failure(
                    reserveResult.Error);
            }

            var reservation = reserveResult.Value!;

            if (!reservation.Success)
            {
                saga.Fail(
                    reservation.Message
                    ?? "Stock reservation failed.");

                await _orderSagaRepository.CompleteAsync(
                    cancellationToken);

                return Result<CheckoutSagaResponse>.Success(
                    new CheckoutSagaResponse(
                        Success: false,
                        Payment: null,
                        ItemsStatus: reservation.ItemsStatus,
                        Message: reservation.Message));
            }

            saga.SetStatus(
                OrderSagaStatus.StockReserved);

            await _orderSagaRepository.CompleteAsync(
                cancellationToken);


            // Initiate Payment   

            saga.SetStatus(
                OrderSagaStatus.PaymentPending);

            await _orderSagaRepository.CompleteAsync(
                cancellationToken);

            var paymentResult =
                await _paymentServiceClient.InitiatePaymentAsync(order.Id, customerId,
                    order.TotalAmount,
                    cancellationToken);

            if (paymentResult.IsFailure)
            {
                saga.SetStatus(
                    OrderSagaStatus.Compensating);

                await _orderSagaRepository.CompleteAsync(
                    cancellationToken);

                return Result<CheckoutSagaResponse>.Failure(
                    paymentResult.Error);
            }

            return Result<CheckoutSagaResponse>.Success(
                new CheckoutSagaResponse(
                    Success: true,
                    Payment: paymentResult.Value!,
                    ItemsStatus: reservation.ItemsStatus,
                    Message: null));
        }

    }
}
