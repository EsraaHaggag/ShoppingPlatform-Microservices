using BuildingBlocks.Common;
using MediatR;

namespace OrderService.Application.Features.Carts.Commands.RemoveItem
{
    public record RemoveCartItemCommand(Guid ProductId) : IRequest<Result>;
}
