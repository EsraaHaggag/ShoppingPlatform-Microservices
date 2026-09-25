using BuildingBlocks.Common;
using BuildingBlocks.Interfaces;
using MediatR;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities.Carts.Errors;

namespace OrderService.Application.Features.Carts.Commands.UpdateCartItemQuantity
{
    public class UpdateCartItemQuantityHandler
    : IRequestHandler<UpdateCartItemQuantityCommand, Result>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductServiceClient _productServiceClient;
        private readonly ICurrentUserService _currentUserService;
        public UpdateCartItemQuantityHandler(
            ICartRepository cartRepository, IProductServiceClient productServiceClient, ICurrentUserService currentUserService)
        {
            _cartRepository = cartRepository;
            _productServiceClient = productServiceClient;
            _currentUserService = currentUserService;
        }

        public async Task<Result> Handle(
        UpdateCartItemQuantityCommand request,
        CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;

            var cart = await _cartRepository.GetByCustomerIdAsync(currentUserId,
                cancellationToken);

            if (cart is null)
                return Result.Failure(CartErrors.EmptyCart);
            var result = cart.UpdateQuantity(request.ProductId, request.quantity);

            if (result.IsFailure)
                return result;

            await _cartRepository.CompleteAsync(
                cancellationToken);

            return Result.Success();
        }
    }
}
