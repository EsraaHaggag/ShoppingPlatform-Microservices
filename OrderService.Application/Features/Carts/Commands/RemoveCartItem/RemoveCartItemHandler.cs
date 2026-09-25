using BuildingBlocks.Common;
using BuildingBlocks.Interfaces;
using MediatR;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities.Carts.Errors;

namespace OrderService.Application.Features.Carts.Commands.RemoveItem
{
    public class RemoveCartItemHandler
    : IRequestHandler<RemoveCartItemCommand, Result>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductServiceClient _productServiceClient;
        private readonly ICurrentUserService _currentUserService;
        public RemoveCartItemHandler(
            ICartRepository cartRepository, IProductServiceClient productServiceClient, ICurrentUserService currentUserService)
        {
            _cartRepository = cartRepository;
            _productServiceClient = productServiceClient;
            _currentUserService = currentUserService;
        }

        public async Task<Result> Handle(
        RemoveCartItemCommand request,
        CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;

            var cart = await _cartRepository.GetByCustomerIdAsync(currentUserId,
                cancellationToken);

            if (cart is null)
                return Result.Failure(CartErrors.EmptyCart);
            var result = cart.RemoveItem(request.ProductId);

            if (result.IsFailure)
                return result;

            await _cartRepository.CompleteAsync(
                cancellationToken);

            return Result.Success();
        }
    }
}

