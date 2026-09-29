using BuildingBlocks.Common;
using BuildingBlocks.Interfaces;
using MediatR;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities.Orders.Errors;

namespace OrderService.Application.Features.Orders.Commands.CancelOrder
{
    public class CancelOrderHandler
    : IRequestHandler<CancelOrderCommand, Result>
    {

        private readonly IOrderRepository _orderRepository;
        private readonly ICurrentUserService _currentUserService;
        public CancelOrderHandler(
            ICurrentUserService currentUserService, IOrderRepository orderRepository)
        {

            _currentUserService = currentUserService;
            _orderRepository = orderRepository;
        }

        public async Task<Result> Handle(CancelOrderCommand request,
         CancellationToken cancellationToken)
        {
            var customerId = _currentUserService.UserId;

            var order = await _orderRepository.GetByIdAsync(request.OrderId, customerId, cancellationToken);

            if (order == null)
                return Result.Failure(OrderErrors.OrderNotFound);
            var orderResult = order.Cancel();

            if (orderResult.IsFailure)
                return Result.Failure(orderResult.Error);
            await _orderRepository.CompleteAsync(
                cancellationToken);
            return Result.Success();

        }
    }
}
