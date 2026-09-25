using BuildingBlocks.Common;
using MediatR;

namespace OrderService.Application.Features.Carts.Commands.UpdateCartItemQuantity
{
    public record UpdateCartItemQuantityCommand(Guid ProductId, int quantity) : IRequest<Result>;
}
