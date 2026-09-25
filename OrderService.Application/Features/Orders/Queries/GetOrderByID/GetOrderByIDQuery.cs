using BuildingBlocks.Common;
using MediatR;
using OrderService.Application.Features.DTOs;

namespace OrderService.Application.Features.Orders.Queries.GetOrderByID
{
    public record GetOrderByIDQuery(Guid orderID) : IRequest<Result<OrderDto>>;
}
