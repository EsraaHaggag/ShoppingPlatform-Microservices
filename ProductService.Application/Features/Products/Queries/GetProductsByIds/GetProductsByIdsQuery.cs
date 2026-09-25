using BuildingBlocks.Common;
using MediatR;
using ProductService.Application.Features.Products.DTOs;

namespace ProductService.Application.Features.Products.Queries.GetProductsByIds
{
    public record GetProductsByIdsQuery(
    IReadOnlyCollection<Guid> ProductIds) : IRequest<Result<IReadOnlyList<ProductInfoDto>>>;
}
