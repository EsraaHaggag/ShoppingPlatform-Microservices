using BuildingBlocks.Common;
using MediatR;
using OrderService.Application.Features.DTOs;

namespace OrderService.Application.Features.Orders.Queries.GetMyOrders
{
    public record GetMyOrdersQuery() : IRequest<Result<List<OrderDto>>>;
}
