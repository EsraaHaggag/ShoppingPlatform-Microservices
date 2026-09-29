using BuildingBlocks.Common;
using MediatR;

namespace ProductService.Application.Features.Products.Commands.CreateProduct
{
    public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int StockQuantity) : IRequest<Result<Guid>>;
}
