using BuildingBlocks.Common;
using MediatR;
using ProductService.Application.Features.Products.DTOs;

namespace ProductService.Application.Features.Products.Queries.GetProductById
{
    public record GetProductByIdQuery(Guid Id) : IRequest<Result<ProductDTO>>;
}
