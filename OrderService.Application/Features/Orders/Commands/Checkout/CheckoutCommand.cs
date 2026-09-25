using BuildingBlocks.Common;
using MediatR;

namespace OrderService.Application.Features.Orders.Commands.Checkout
{
    public record CheckoutCommand() : IRequest<Result<Guid>>;
}
