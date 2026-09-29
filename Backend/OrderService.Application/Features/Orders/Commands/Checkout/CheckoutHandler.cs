using BuildingBlocks.Common;
using BuildingBlocks.Interfaces;
using MediatR;
using OrderService.Application.Interfaces;
using OrderService.Application.Sagas;
using OrderService.Domain.Entities.Carts.Errors;
using OrderService.Domain.Entities.Orders;

namespace OrderService.Application.Features.Orders.Commands.Checkout
{
    public class CheckoutHandler
    : IRequestHandler<CheckoutCommand, Result<CheckoutSagaResponse>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductServiceClient _productServiceClient;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPaymentServiceClient _paymentServiceClient;
        private readonly IOrderSagaOrchestrator _orderSagaOrchestrator;
        public CheckoutHandler(
            ICartRepository cartRepository, IProductServiceClient productServiceClient, ICurrentUserService currentUserService, IPaymentServiceClient paymentServiceClient,
            IOrderRepository orderRepository, IOrderSagaOrchestrator orderSagaOrchestrator)
        {
            _cartRepository = cartRepository;
            _productServiceClient = productServiceClient;
            _currentUserService = currentUserService;
            _paymentServiceClient = paymentServiceClient;
            _orderRepository = orderRepository;
            _orderSagaOrchestrator = orderSagaOrchestrator;
        }


        public async Task<Result<CheckoutSagaResponse>> Handle(
          CheckoutCommand request,
        CancellationToken cancellationToken)
        {
            var customerId = _currentUserService.UserId;

            var cart = await _cartRepository.GetByCustomerIdAsync(
                customerId,
                cancellationToken);

            if (cart is null || !cart.Items.Any())
            {
                return Result<CheckoutSagaResponse>.Failure(
                    CartErrors.EmptyCart);
            }

            var orderResult = Order.Create(customerId);

            if (orderResult.IsFailure)
            {
                return Result<CheckoutSagaResponse>.Failure(
                    orderResult.Error);
            }

            var order = orderResult.Value!;

            foreach (var cartItem in cart.Items)
            {
                var result = order.AddItem(
                    cartItem.ProductId,
                    cartItem.ProductName,
                    cartItem.UnitPrice,
                    cartItem.Quantity);

                if (result.IsFailure)
                {
                    return Result<CheckoutSagaResponse>.Failure(result.Error);
                }
            }

            await _orderRepository.AddAsync(order);

            await _orderRepository.CompleteAsync(
                cancellationToken);

            // Start Saga
            var sagaResult =
                await _orderSagaOrchestrator.StartAsync(order.Id, customerId,
                    cancellationToken);

            if (sagaResult.IsFailure)
            {
                return Result<CheckoutSagaResponse>.Failure(
                    sagaResult.Error);
            }

            return Result<CheckoutSagaResponse>.Success(sagaResult.Value!);
        }

    }
}
