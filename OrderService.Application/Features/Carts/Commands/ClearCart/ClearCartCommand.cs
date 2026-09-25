using BuildingBlocks.Common;
using MediatR;

namespace OrderService.Application.Features.Carts.Commands.ClearCart
{
    public record ClearCartCommand() : IRequest<Result>;
}
