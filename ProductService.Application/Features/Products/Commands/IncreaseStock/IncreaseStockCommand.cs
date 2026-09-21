using BuildingBlocks.Common;
using MediatR;

namespace ProductService.Application.Features.Products.Commands.IncreaseStock
{
    public record IncreaseStockCommand(Guid Id, int quantity) : IRequest<Result>;

}
