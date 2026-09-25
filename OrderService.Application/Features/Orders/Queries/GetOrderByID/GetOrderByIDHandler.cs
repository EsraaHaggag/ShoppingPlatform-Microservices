
using AutoMapper;
using BuildingBlocks.Common;
using BuildingBlocks.Interfaces;
using MediatR;
using OrderService.Application.Features.DTOs;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities.Orders.Errors;

namespace OrderService.Application.Features.Orders.Queries.GetOrderByID
{
    public class GetOrderByIDHandler
    : IRequestHandler<GetOrderByIDQuery, Result<OrderDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetOrderByIDHandler(
            IOrderRepository orderRepository,
            ICurrentUserService currentUserService, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Result<OrderDto>> Handle(
            GetOrderByIDQuery request,
            CancellationToken cancellationToken)
        {
            var customerId = _currentUserService.UserId;

            var order = await _orderRepository.GetByIdAsync(
                request.orderID,
                customerId,
                cancellationToken);

            if (order is null)
                return Result<OrderDto>.Failure(
                  OrderErrors.OrderNotFound);

            var orderDto = _mapper.Map<OrderDto>(order);
            return Result<OrderDto>.Success(orderDto);
        }
    }
}

