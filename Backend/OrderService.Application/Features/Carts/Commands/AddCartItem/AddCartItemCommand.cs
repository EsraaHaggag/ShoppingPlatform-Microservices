using BuildingBlocks.Common;
using MediatR;

namespace OrderService.Application.Features.Carts.Commands.AddCartItem
{
    public record AddCartItemCommand(Guid ProductId,
    int Quantity) : IRequest<Result>;
}
