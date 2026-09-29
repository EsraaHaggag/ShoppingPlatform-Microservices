using BuildingBlocks.Common;
using MediatR;

namespace ProductService.Application.Features.Products.Commands.Deactivate
{
    public record DeactivateCommand(Guid Id) : IRequest<Result>;
}
