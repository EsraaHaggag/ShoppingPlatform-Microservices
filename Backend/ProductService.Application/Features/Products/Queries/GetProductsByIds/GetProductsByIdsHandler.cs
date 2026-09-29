using AutoMapper;
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
        private readonly IProductCacheService _productCacheService;
        private readonly IMapper _mapper;

        public GetProductsByIdsHandler(
            IProductRepository productRepository,
            IProductCacheService productCacheService,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _productCacheService = productCacheService;
            _mapper = mapper;
        }

        public async Task<Result<IReadOnlyList<ProductInfoDto>>> Handle(GetProductsByIdsQuery request,
            CancellationToken cancellationToken)
        {
            var cachedProducts =
                await _productCacheService.GetManyAsync(request.ProductIds,
                    cancellationToken);

            var cachedProductIds = cachedProducts
                .Select(p => p.Id)
                .ToHashSet();

            var missingProductIds = request.ProductIds
                .Where(id => !cachedProductIds.Contains(id)).ToList();

            var products = cachedProducts.ToList();
            if (missingProductIds.Count > 0)
            {
                var databaseProducts =
                    await _productRepository.GetByIdsAsync(
                        missingProductIds,
                        cancellationToken);

                foreach (var product in databaseProducts)
                {
                    var productDto =
                        _mapper.Map<ProductDTO>(product);

                    products.Add(productDto);
                    await _productCacheService.SetAsync(productDto,
                       cancellationToken);
                }
            }

            var result = _mapper.Map<List<ProductInfoDto>>(products);

            return Result<IReadOnlyList<ProductInfoDto>>.Success(result);
        }
    }
}

