using BuildingBlocks.Common;
using MediatR;

namespace ProductService.Application.Features.Products.Commands.DecreaseStock
{
    public record DecreaseStockCommand(Guid Id, int quantity) : IRequest<Result>;
}
