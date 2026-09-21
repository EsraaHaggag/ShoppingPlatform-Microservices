using BuildingBlocks.Common;
using MediatR;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities.Carts;
using OrderService.Domain.Entities.Carts.Errors;

namespace OrderService.Application.Features.Carts.Commands.AddCartItem
{
    public class AddCartItemHandler
    : IRequestHandler<AddCartItemCommand, Result>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductServiceClient _productServiceClient;

        public AddCartItemHandler(
            ICartRepository cartRepository, IProductServiceClient productServiceClient)
        {
            _cartRepository = cartRepository;
            _productServiceClient = productServiceClient;
        }

        public async Task<Result> Handle(
        AddCartItemCommand request,
        CancellationToken cancellationToken)
        {
            var productResult = await _productServiceClient.GetProductAsync(request.ProductId,
                    cancellationToken);

            if (productResult.IsFailure)
                return Result.Failure(productResult.Error);

            var product = productResult.Value!;

            if (request.Quantity > product.StockQuantity)
            {
                return Result.Failure(
                CartErrors.InsufficientStock);
            }

            var cart = await _cartRepository.GetByCustomerIdAsync(request.CustomerId,
                cancellationToken);

            if (cart is null)
            {
                var cartResult =
                    Cart.Create(request.CustomerId);

                if (cartResult.IsFailure)
                    return Result.Failure(cartResult.Error);

                cart = cartResult.Value!;
                await _cartRepository.AddAsync(cart);
            }

            var result = cart.AddItem(
                product.Id,
                product.Name,
                product.Price,
                request.Quantity);

            if (result.IsFailure)
                return result;

            await _cartRepository.CompleteAsync(
                cancellationToken);

            return Result.Success();
        }
    }
}
