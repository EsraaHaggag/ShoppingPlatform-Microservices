using BuildingBlocks.Common;
using BuildingBlocks.Interfaces;
using MediatR;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities.Carts.Errors;

namespace OrderService.Application.Features.Carts.Commands.ClearCart
{
    public class ClearCartHandler
    : IRequestHandler<ClearCartCommand, Result>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ICartRepository _cartRepository;
        public ClearCartHandler(
         ICurrentUserService currentUserService, ICartRepository cartRepository)
        {
            _currentUserService = currentUserService;
            _cartRepository = cartRepository;
        }

        public async Task<Result> Handle(
        ClearCartCommand request,
        CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;

            var cart = await _cartRepository.GetByCustomerIdAsync(currentUserId,
                cancellationToken);

            if (cart is null)
                return Result.Failure(CartErrors.EmptyCart);
            cart.Clear();
            await _cartRepository.CompleteAsync(
                cancellationToken);

            return Result.Success();
        }
    }
}
