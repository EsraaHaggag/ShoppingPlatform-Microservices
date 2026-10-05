using BuildingBlocks.Common;
using BuildingBlocks.Interfaces;
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
        private readonly ICurrentUserService _currentUserService;

        public AddCartItemHandler(
            ICartRepository cartRepository, IProductServiceClient productServiceClient,
            ICurrentUserService currentUserService)
        {
            _cartRepository = cartRepository;
            _productServiceClient = productServiceClient;
            _currentUserService = currentUserService;
        }

        public async Task<Result> Handle(
        AddCartItemCommand request,
        CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;
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

            var cart = await _cartRepository.GetByCustomerIdAsync(currentUserId, cancellationToken);

            if (cart is null)
            {
                var cartResult =
                    Cart.Create(currentUserId);

                if (cartResult.IsFailure)
                    return Result.Failure(cartResult.Error);

                cart = cartResult.Value!;
                await _cartRepository.AddAsync(cart);
            }
            var primaryImage = product.Images
            .FirstOrDefault(x => x.IsPrimary)?.ImageUrl
            ?? product.Images.FirstOrDefault()?.ImageUrl;
            var result = cart.AddItem(
                product.Id,
                product.Name,
                product.Price,
                request.Quantity,
                primaryImage);

            if (result.IsFailure)
                return Result.Failure(result.Error);

            await _cartRepository.CompleteAsync(
                cancellationToken);

            return Result.Success();
        }
    }
}
