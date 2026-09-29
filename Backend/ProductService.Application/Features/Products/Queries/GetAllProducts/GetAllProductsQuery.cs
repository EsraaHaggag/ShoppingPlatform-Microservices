using BuildingBlocks.Common;
using MediatR;
using ProductService.Application.Features.Products.DTOs;

namespace ProductService.Application.Features.Products.Queries.GetAllProduct
{
    public record GetAllProductsQuery
     : IRequest<Result<PaginatedResult<ProductDTO>>>
    {
        public decimal? MinPrice { get; init; }
        public decimal? MaxPrice { get; init; }
        public string? SortBy { get; init; }
        public string? SortDirection { get; init; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
