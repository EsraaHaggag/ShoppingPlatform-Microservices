using BuildingBlocks.Common;
using BuildingBlocks.Stock;
using MediatR;

namespace ProductService.Application.Features.Products.Commands.ReserveStock
{
    public record ReserveStockCommand(
    IReadOnlyList<ReserveStockItem> Items)
    : IRequest<Result<ReserveStockResponse>>;
}
