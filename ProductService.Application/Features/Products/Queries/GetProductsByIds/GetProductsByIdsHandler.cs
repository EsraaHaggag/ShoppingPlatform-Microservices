using BuildingBlocks.Common;
using MediatR;
using ProductService.Application.Features.Products.DTOs;
using ProductService.Application.Interfaces;

namespace ProductService.Application.Features.Products.Queries.GetProductsByIds
{
    public class GetProductsByIdsHandler
     : IRequestHandler<
     GetProductsByIdsQuery,
    Result<IReadOnlyList<ProductInfoDto>>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsByIdsHandler(
            IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<IReadOnlyList<ProductInfoDto>>> Handle(
            GetProductsByIdsQuery request,
            CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetByIdsAsync(
                request.ProductIds,
                cancellationToken);

            var result = products
                .Select(p => new ProductInfoDto(
                    p.Id,
                    p.Name,
                    p.Price,
                    p.StockQuantity))
                .ToList();

            return Result<IReadOnlyList<ProductInfoDto>>.Success(result);
        }
    }
}

