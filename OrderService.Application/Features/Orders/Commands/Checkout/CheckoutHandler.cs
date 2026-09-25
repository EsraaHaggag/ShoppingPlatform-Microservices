using BuildingBlocks.Common;
using BuildingBlocks.Interfaces;
using MediatR;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities.Carts.Errors;
using OrderService.Domain.Entities.Orders;

namespace OrderService.Application.Features.Orders.Commands.Checkout
{
    public class CheckoutHandler
    : IRequestHandler<CheckoutCommand, Result<Guid>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductServiceClient _productServiceClient;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPaymentServiceClient _paymentServiceClient;
        public CheckoutHandler(
            ICartRepository cartRepository, IProductServiceClient productServiceClient, ICurrentUserService currentUserService, IPaymentServiceClient paymentServiceClient,
            IOrderRepository orderRepository)
        {
            _cartRepository = cartRepository;
            _productServiceClient = productServiceClient;
            _currentUserService = currentUserService;
            _paymentServiceClient = paymentServiceClient;
            _orderRepository = orderRepository;
        }

        public async Task<Result<Guid>> Handle(
        CheckoutCommand request,
        CancellationToken cancellationToken)
        {
            var customerId = _currentUserService.UserId;

            var cart = await _cartRepository.GetByCustomerIdAsync(
                customerId,
                cancellationToken);

            if (cart is null || !cart.Items.Any())
                return Result<Guid>.Failure(CartErrors.EmptyCart);

            var productIds = cart.Items
                .Select(x => x.ProductId)
                .ToList();

            var productsResult = await _productServiceClient.GetProductsAsync(
                productIds,
                cancellationToken);

            if (productsResult.IsFailure)
                return Result<Guid>.Failure(productsResult.Error);

            var products = productsResult.Value!;

            var productDict = products.ToDictionary(p => p.Id);

            foreach (var cartItem in cart.Items)
            {
                if (!productDict.TryGetValue(
                        cartItem.ProductId,
                        out var product))
                {
                    return Result<Guid>.Failure(
                        CartErrors.ProductNotFound);
                }

                if (product.Price != cartItem.UnitPrice)
                {
                    return Result<Guid>.Failure(
                        CartErrors.PriceChanged);
                }

                if (cartItem.Quantity > product.StockQuantity)
                {
                    return Result<Guid>.Failure(
                        CartErrors.InsufficientStock);
                }
            }

            var totalAmount = cart.Items.Sum(
                x => x.UnitPrice * x.Quantity);

            var orderResult = Order.Create(customerId);

            if (orderResult.IsFailure)
                return Result<Guid>.Failure(orderResult.Error);

            var order = orderResult.Value!;

            foreach (var cartItem in cart.Items)
            {
                var result = order.AddItem(
                    cartItem.ProductId,
                    cartItem.ProductName,
                    cartItem.UnitPrice,
                    cartItem.Quantity);

                if (result.IsFailure)
                    return Result<Guid>.Failure(result.Error);
            }

            await _orderRepository.AddAsync(order);

            await _orderRepository.CompleteAsync(
                cancellationToken);

            // HTTP → Payment Service
            var paymentResult =
                await _paymentServiceClient.InitiatePaymentAsync(order.Id, customerId, totalAmount,
                    cancellationToken);

            if (paymentResult.IsFailure)
                return Result<Guid>.Failure(paymentResult.Error);


            return Result<Guid>.Success(order.Id);
        }
    }
}
