using AutoMapper;
using BuildingBlocks.Common;
using BuildingBlocks.Interfaces;
using MediatR;
using OrderService.Application.Features.DTOs;
using OrderService.Application.Interfaces;

namespace OrderService.Application.Features.Orders.Queries.GetMyOrders
{
    public class GetMyOrdersHandler
    : IRequestHandler<GetMyOrdersQuery, Result<List<OrderDto>>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetMyOrdersHandler(
            IOrderRepository orderRepository,
            ICurrentUserService currentUserService, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Result<List<OrderDto>>> Handle(
            GetMyOrdersQuery request,
            CancellationToken cancellationToken)
        {
            var customerId = _currentUserService.UserId;

            var orders = await _orderRepository.GetByCustomerIdAsync(customerId,
                cancellationToken);

            var ordersDto = _mapper.Map<List<OrderDto>>(orders);
            return Result<List<OrderDto>>.Success(ordersDto);
        }
    }
}
