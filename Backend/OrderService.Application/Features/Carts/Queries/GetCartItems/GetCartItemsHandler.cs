using AutoMapper;
using BuildingBlocks.Common;
using BuildingBlocks.Interfaces;
using MediatR;
using OrderService.Application.Features.DTOs;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities.Carts.Errors;


namespace OrderService.Application.Features.Carts.Queries.GetCartItems
{
    public class GetCartItemsHandler : IRequestHandler<GetCartItemsQuery, Result<CartDto>>
    {
        private readonly IMapper _mapper;
        private readonly ICartRepository _cartRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetCartItemsHandler(IMapper mapper, ICartRepository cartRepository, ICurrentUserService currentUserService)
        {
            _cartRepository = cartRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Result<CartDto>> Handle(
            GetCartItemsQuery request,
            CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;

            var cart = await _cartRepository.GetByCustomerIdAsync(currentUserId,
                cancellationToken);

            if (cart is null)
                return Result<CartDto>.Failure(CartErrors.EmptyCart);

            var cartDto = _mapper.Map<CartDto>(cart);

            return Result<CartDto>.Success(cartDto);
        }
    }
}
