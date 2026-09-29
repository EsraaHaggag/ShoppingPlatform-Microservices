using BuildingBlocks.Common;
using MediatR;
using OrderService.Application.Features.DTOs;

namespace OrderService.Application.Features.Carts.Queries.GetCartItems
{
    public record GetCartItemsQuery() : IRequest<Result<CartDto>>;
}
