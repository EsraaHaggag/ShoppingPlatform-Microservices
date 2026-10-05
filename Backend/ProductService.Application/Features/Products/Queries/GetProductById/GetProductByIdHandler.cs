using AutoMapper;
using BuildingBlocks.Common;
using MediatR;
using ProductService.Application.Features.Products.DTOs;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities.Products.Errors;

namespace ProductService.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdHandler
    : IRequestHandler<GetProductByIdQuery, Result<ProductDTO>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCacheService _productCacheService;
        private readonly IMapper _mapper;

        public GetProductByIdHandler(
            IProductRepository productRepository, IMapper mapper, IProductCacheService productCacheService)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _productCacheService = productCacheService;
        }

        public async Task<Result<ProductDTO>> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
        {
            var cachedProduct = await _productCacheService.GetAsync(
                request.Id,
                cancellationToken);

            if (cachedProduct is not null)
                return Result<ProductDTO>.Success(cachedProduct);

            var result = await _productRepository.GetByIdAsync(
                request.Id,
                cancellationToken);

            if (result is null)
                return Result<ProductDTO>.Failure(
                    ProductErrors.ProductNotFound);

            var productDto = _mapper.Map<ProductDTO>(result);

            await _productCacheService.SetAsync(
                productDto,
                cancellationToken);

            return Result<ProductDTO>.Success(productDto);
        }

    }
}
