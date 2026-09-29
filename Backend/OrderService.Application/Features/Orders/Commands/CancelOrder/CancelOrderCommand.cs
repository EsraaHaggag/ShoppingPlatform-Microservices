using BuildingBlocks.Common;
using MediatR;

namespace OrderService.Application.Features.Orders.Commands.CancelOrder
{
    public record CancelOrderCommand(Guid OrderId) : IRequest<Result>;
}
