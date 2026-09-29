using BuildingBlocks.Common;
using MediatR;

namespace ProductService.Application.Features.Products.Commands.Reactivate
{
    public record ReactivateCommand(Guid Id) : IRequest<Result>;
}
