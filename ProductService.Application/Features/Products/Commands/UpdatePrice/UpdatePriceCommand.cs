using BuildingBlocks.Common;
using MediatR;

namespace ProductService.Application.Features.Products.Commands.UpdatePrice
{
    public record UpdatePriceCommand(Guid Id, decimal Price) : IRequest<Result>;
}
